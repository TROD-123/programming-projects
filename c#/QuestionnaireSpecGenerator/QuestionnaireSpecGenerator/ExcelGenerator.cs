using System.Drawing;
using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Marshal = System.Runtime.InteropServices.Marshal;


namespace QuestionnaireSpecGenerator
{
    internal class ExcelGenerator
    {
        private Application app = null;         // for creating the excel environment/process
        private Workbook workbook = null;       // for creating the workbook file
        private Worksheet worksheet = null;     // for allowing us to work in the current worksheet
        private Range worksheetRange = null;    // for allowing us to modify cells on the sheet

        private int currentSheet = 0;

        // constructor. initializes the excel variables above. needs to be called in the program
        public ExcelGenerator()
        {
            createAppInstance();
        }

        #region Private Methods

        private void createAppInstance()
        {
            try
            {
                app = new Application();
                workbook = app.Workbooks.Add();
            }
            catch (Exception e)
            {
                Console.Write("Error");
            }
            finally
            {
            }
        }

        private void releaseObject(object obj) // note ref!
        {
            // Do not catch an exception from this.
            // You may want to remove these guards depending on
            // what you think the semantics should be.
            if (obj != null && Marshal.IsComObject(obj))
            {
                Marshal.ReleaseComObject(obj);
            }
            // Since passed "by ref" this assingment will be useful
            // (It was not useful in the original, and neither was the
            //  GC.Collect.)
            obj = null;
        }

        #region PRIVATE: Creating modules

        // note: keep the excel window hidden while it is populated
        // TODO: This should be redefined for a MODULE. The SECTION stuff can be split into its own method
        private void createSection()
        {
            if (currentSheet == 0)
            {
                worksheet = (Worksheet)workbook.Sheets[1];
                currentSheet++;
            }
            else
            {
                // otherwise, add the new sheet after the previous one
                worksheet = (Worksheet)workbook.Sheets.Add(After: workbook.Sheets[workbook.Sheets.Count]);
            }

            app.StandardFont = Constants.defaultFont;
            app.StandardFontSize = Constants.defaultFontSize;


            setDefaultColumnWidths();
            setDefaultRowHeights();
        }

        // set default column widths
        private void setDefaultColumnWidths()
        {
            worksheetRange = worksheet.get_Range("A:A");
            worksheetRange.ColumnWidth = Constants.leftColSize;

            worksheetRange = worksheet.get_Range("B:B");
            worksheetRange.ColumnWidth = Constants.rightColSize;

            worksheetRange = null;
        }

        // set default row height
        private void setDefaultRowHeights()
        {
            worksheetRange = worksheet.Cells; // gets all the cells in the worksheet
            worksheetRange.RowHeight = Constants.defaultRowHeight;

            worksheetRange = null;
        }

        #endregion

        #region PRIVATE: Cell formatting helpers

        // set the fill color of the cell/range
        private void formatCellColor(Range worksheetRange, string color)
        {
            ColorConverter cv = new ColorConverter();
            switch (color)
            {
                case Constants.Colors.LIGHT_GREEN:
                    worksheetRange.Interior.Color = (Color)cv.ConvertFromString(Constants.Colors.LIGHT_GREEN);
                    break;
                case Constants.Colors.DARK_GREEN:
                    worksheetRange.Interior.Color = (Color)cv.ConvertFromString(Constants.Colors.DARK_GREEN);
                    break;
                case Constants.Colors.GREY:
                    worksheetRange.Interior.Color = (Color)cv.ConvertFromString(Constants.Colors.GREY);
                    break;
                default:
                    //cellRange.Interior.Color = (Color) cv.ConvertFromString();
                    break;
            }
        }

        // set the text color of the cell/range
        private void formatCellTextColor(Range worksheetRange, string color)
        {
            if (color == Constants.Colors.WHITE)
            {
                ColorConverter cv = new ColorConverter();
                worksheetRange.Font.Color = (Color)cv.ConvertFromString(color);
            }

        }

        // set the alignment of text in cell
        private void formatCellAlignment(Range worksheetRange, XlHAlign hAlign, XlVAlign vAlign)
        {
            worksheetRange.HorizontalAlignment = hAlign;
            worksheetRange.VerticalAlignment = vAlign;
        }

        // format the border
        private void formatCellBorder(Range worksheetRange, RowType rowType)
        {
            if (rowType == RowType.None)
            {
                // set a thick box border around range if RowType isn't specified
                worksheetRange.BorderAround2(Type.Missing, XlBorderWeight.xlMedium, XlColorIndex.xlColorIndexAutomatic, Type.Missing);
                return;
            }

            if (rowType != RowType.SectionHeader)
            {
                // do not set border if RowType is Section Header
                worksheetRange.Borders.Color = Color.Black.ToArgb();
            }
        }

        // set font formatting
        private void formatCellFont(Range worksheetRange, TextFormats textFormat)
        {
            switch (textFormat)
            {
                case TextFormats.Normal:
                    // do nothing
                    break;
                case TextFormats.Bold:
                    worksheetRange.Font.Bold = true;
                    break;
                case TextFormats.Italics:
                    worksheetRange.Font.Italic = true;
                    break;
                case TextFormats.Strikethrough:
                    worksheetRange.Font.Strikethrough = true;
                    break;
                default:
                    break;
            }
        }

        #endregion

        // set cell text
        private void setCellText(int row, int col, string text)
        {
            worksheet.Cells[row, col] = text;
        }

        // create specific row based on RowType
        private void createRow(int startRow, int startCol, string leftColText, string rightColText, RowType rowType)
        {
            int endCol = startCol + Constants.defaultColumnOffset;

            // contingent values based on rowType. 

            // statically set defaults
            TextFormats textFormat = RowTypeArrays.textFormats[(int)rowType];
            string cellColor = RowTypeArrays.cellColors[(int)rowType];
            string textColor = Constants.Colors.BLACK; // by default color is black
            bool mergeCells = false; // by default, do not merge cells
            bool separateCellFormatting = false; // by default, both cells formatted the same

            // dynamically set rowType chars
            if (rowType == RowType.SectionHeader)
            {
                // if RowType is Section Header, then leftColText is Section Letter and rightColText is Section Title
                leftColText = String.Format("SECTION {0}: {1}", leftColText, rightColText);
                rightColText = null;
                mergeCells = true;
                // override default colors
                textColor = Constants.Colors.WHITE;
            }
            else if (rowType == RowType.Response)
            {
                // if RowType is Response, then col texts are given. format both cells differently
                separateCellFormatting = true;
            }
            else if (rowType == RowType.Code_Generic || rowType == RowType.Code_Column || rowType == RowType.Code_Row)
            {
                // if RowType is a code, set text by predefined array and format both cells differently
                leftColText = RowTypeArrays.leftColStrings[(int)rowType];
                separateCellFormatting = true;
            }
            else if (rowType != RowType.QuestionTitle)
            {
                // if RowType is not QuestionText, then set text by predefined array
                leftColText = RowTypeArrays.leftColStrings[(int)rowType];
            }


            // set the text of the cell
            worksheet.Cells[startRow, startCol] = leftColText;
            if (rightColText != null)
            {
                worksheet.Cells[startRow, endCol] = rightColText;
            }

            // define the range for formatting and set formatting
            if (separateCellFormatting)
            {
                worksheetRange = worksheet.get_Range(String.Format("{0}{1}", Toolbox.GetExcelColumnName(startCol), startRow));
                formatCellAlignment(worksheetRange, XlHAlign.xlHAlignCenter, XlVAlign.xlVAlignBottom);
            }

            worksheetRange = worksheet.get_Range(String.Format("{0}{2}:{1}{2}", Toolbox.GetExcelColumnName(startCol), Toolbox.GetExcelColumnName(endCol), startRow));
            formatCellFont(worksheetRange, textFormat);
            formatCellTextColor(worksheetRange, textColor);
            formatCellColor(worksheetRange, cellColor);
            formatCellBorder(worksheetRange, rowType);
            if (mergeCells)
            {
                worksheetRange.Merge();
                formatCellAlignment(worksheetRange, XlHAlign.xlHAlignCenter, XlVAlign.xlVAlignCenter);
                worksheetRange.RowHeight = Constants.defaultSectionRowHeight;
                worksheetRange.Font.Size = Constants.defaultSectionFontSize;
            }
        }

        #endregion

        #region Public Methods

        // set visibility of window
        public void SetWindowVisibility(bool visible)
        {
            app.Visible = visible;
        }

        // from https://stackoverflow.com/questions/158706/how-do-i-properly-clean-up-excel-interop-objects
        public void CleanUpExcelInteropObjs()
        {
            // Release all COM RCWs.
            // The "releaseObject" will just "do nothing" if null is passed,
            // so no need to check to find out which need to be released.
            // The "finally" is run in all cases, even if there was an exception
            // in the "try". 
            // Note: passing "by ref" so afterwords "xlWorkSheet" will
            // evaluate to null. See "releaseObject".
            releaseObject(worksheet);
            releaseObject(workbook);
            // The Quit is done in the finally because we always
            // want to quit. It is no different than releasing RCWs.
            if (app != null)
            {
                app.Quit();
            }
            releaseObject(app);
        }

        // create Section Header
        public int CreateSectionHeader(int startRow, int startCol, string sectionLetter, string sectionTitle)
        {
            int currentRow = startRow;

            createRow(currentRow++, startCol, sectionLetter, sectionTitle, RowType.SectionHeader);

            // Add an empty row below section header
            return ++currentRow;
        }

        // create new sheet with new section
        // TODO: This needs to be for a MODULE, not a section
        public int CreateNewSheetSectionHeader(string sectionLetter, string sectionTitle)
        {
            createSection();
            workbook.Worksheets[currentSheet].Name = String.Format("SECTION {0}", sectionLetter);
            currentSheet++;

            return CreateSectionHeader(Constants.defaultFirstRow, Constants.defaultFirstColumn, sectionLetter, sectionTitle);
        }

        // create info module of question block
        // assumes: rightColStrArray is aligned with rowTypeArr
        // return the number of the last row
        public int CreateInfoModule(int startRow, int startCol, string qNum, string[] rightColStrArr)
        {
            int currentRow = startRow;
            RowType[] rowTypeArr = (RowType[])Enum.GetValues(typeof(RowType));

            for (int i = 0; i < rightColStrArr.Length; i++)
            {
                createRow(currentRow++, Constants.defaultFirstColumn, qNum, rightColStrArr[i], rowTypeArr[i + 1]); // skip RowType.SectionHeader
            }

            return currentRow;
        }

        // response options
        // counting assumes responseArray is in the form of:
        //      FIELD:      { GENERIC, "Special Response A", [...] }
        //      GENERIC:    { GENERIC, "Response A", "Response B", "Response C", [...] }
        //      ROW/COLUMN: { COLUMN, "Response A", "Response B", "Response C", [...], ROW, "Response A", "Response B", "Response C", [...] }
        public int CreateResponseModule(int startRow, int startCol, string[] responseArray, QuestionType qType)
        {
            int currentRow = startRow;

            int currentCode = Constants.ResponseCodes.firstRow; // assume response rows by default;

            for (int i = 0; i < responseArray.Length; i++)
            {
                string leftColText = "";
                string rightColText = "";
                RowType rowType = RowType.Response;

                switch (responseArray[i])
                {
                    // no value expected in right column
                    case Constants.ResponseCodes.GENERIC:
                        rowType = RowType.Code_Generic;
                        currentCode = Constants.ResponseCodes.firstRow;
                        break;
                    case Constants.ResponseCodes.COLUMN:
                        rowType = RowType.Code_Column;
                        currentCode = Constants.ResponseCodes.firstColumn;
                        break;
                    case Constants.ResponseCodes.ROW:
                        rowType = RowType.Code_Row;
                        currentCode = Constants.ResponseCodes.firstRow;
                        break;

                    // special codes
                    case Constants.SpecialResponses.DONT_KNOW:

                    case Constants.SpecialResponses.PREFER_NO_ANSWER:

                    case Constants.SpecialResponses.PREFER_NO_SAY:
                        leftColText = Constants.ResponseCodes.idk.ToString();
                        rightColText = responseArray[i];
                        break;
                    case Constants.SpecialResponses.OTHER:

                    case Constants.SpecialResponses.OTHER_SPECIFY:
                        leftColText = Constants.ResponseCodes.other.ToString();
                        rightColText = responseArray[i];
                        break;
                    case Constants.SpecialResponses.ALL_OF_THE_ABOVE:
                        leftColText = Constants.ResponseCodes.all.ToString();
                        rightColText = responseArray[i];
                        break;
                    case Constants.SpecialResponses.NONE_OF_THE_ABOVE:
                        leftColText = Constants.ResponseCodes.none.ToString();
                        rightColText = responseArray[i];
                        break;
                    default:
                        leftColText = currentCode.ToString();
                        rightColText = responseArray[i];
                        currentCode++;
                        break;
                }

                createRow(currentRow++, Constants.defaultFirstColumn, leftColText, rightColText, rowType);
            }

            return currentRow;
        }


        // wraps a thick box border around the question block
        public void BorderQuestionBlock(int startRow, int endRow, int startCol)
        {
            string range = String.Format("{0}{1}:{2}{3}", Toolbox.GetExcelColumnName(startCol), startRow, Toolbox.GetExcelColumnName(startCol + 1), endRow);
            worksheetRange = worksheet.get_Range(range);
            formatCellBorder(worksheetRange, RowType.None);
        }

        // display the first sheet
        public void ShowFirstSheet()
        {
            worksheet = (Worksheet)workbook.Sheets[1];
            worksheet.Activate();
        }

        #endregion Public Methods

    }
}
