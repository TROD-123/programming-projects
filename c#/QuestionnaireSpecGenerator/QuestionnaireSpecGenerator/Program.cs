using System.Drawing;
using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;

namespace QuestionnaireSpecGenerator
{
    class Program
    {
        // when run, an excel file gets created and populated with the below "specs". Sheet is not saved
        // anywhere, so must be saved manually once it is made.
        //
        // creates a new spreadsheet each time program is run. code continues to run in the background
        // after the excel sheet is created, and then 'takes control' of the recently created excel doc
        // and manipulates it as spec'd
        public static void Main(string[] args)
        {
            //List<Response> _data = new List<Response>();
            //_data.Add(new Response()
            //{
            //    Code = 1,
            //    RespText = "Mark here if this does not apply",
            //    Flags = null,
            //    RType = ResponseType.None
            //});

            ////open file stream
            //using (StreamWriter file = File.CreateText(@"D:\path.txt"))
            //{
            //    JsonSerializer serializer = new JsonSerializer();
            //    //serialize object directly into file stream
            //    serializer.Serialize(file, _data);
            //}

            Questionnaire qre = JsonHandler.DeserializeJsonFromFile(@".\..\..\json_qreTest.json");

            //Tester.TestModifyQreExistingObjectsIndirectly(qre);
            //Tester.TestRandomIntGenerator(qre);
            //Tester.TestGetObjectById(qre);
            Tester.TestCreateQreNoJson();





            //ExcelGenerator excelApp = new ExcelGenerator();

            //// keep track of current row for incrementing
            //int currentRow = Constants.defaultFirstRow;
            //// for tracking the first row of a question block
            //int startRow = currentRow;
            //// for tracking the last row of a question block
            //int endRow = currentRow;

            //// return current row after making section header
            //currentRow = excelApp.CreateNewSheetSectionHeader("A", "SCREENING, QUOTAS, AND SOCIAL BEHAVIOR");

            //// store the question numbers
            //string[] qNumArr = { "ETA10", "ETA20", "ETA30", "ETA40", "ETA50" };

            //for (int i = 0; i < qNumArr.Length; i++)
            //{
            //    startRow = currentRow;
            //    // create info module. return current row after making info module
            //    string[] rightColTextArr = { "Age", "All respondents", "n/a", "Open field", "No", "All respondents", "Open numeric", "How many fingers am I holding?", "Please enter your answer below" };
            //    string[] responseOptionsArr = { Constants.ResponseCodes.GENERIC, "This is the first response", "This is the second response", "Response number three!",
            //        Constants.SpecialResponses.ALL_OF_THE_ABOVE, Constants.SpecialResponses.PREFER_NO_ANSWER, Constants.SpecialResponses.OTHER_SPECIFY, Constants.SpecialResponses.NONE_OF_THE_ABOVE };
            //    currentRow = excelApp.CreateInfoModule(currentRow, Constants.defaultFirstColumn, qNumArr[i], rightColTextArr);
            //    currentRow = excelApp.CreateResponseModule(currentRow, Constants.defaultFirstColumn, responseOptionsArr, QuestionType.Grid);
            //    endRow = currentRow + Constants.rowCountOffset;

            //    excelApp.BorderQuestionBlock(startRow, endRow, 1);

            //    currentRow++;
            //}

            //currentRow = excelApp.CreateNewSheetSectionHeader("B", "THIS IS A TEST");

            //for (int i = 0; i < 3; i++)
            //{
            //    startRow = currentRow;
            //    // create info module. return current row after making info module
            //    string[] rightColTextArr = { "Age", "All respondents", "n/a", "Open field", "No", "All respondents", "Open numeric", "How many fingers am I holding?", "Please enter your answer below" };
            //    string[] responseOptionsArr = { Constants.ResponseCodes.COLUMN, "This is the first response", "This is the second response", "Response number three!", Constants.ResponseCodes.ROW, "Totally agree", "Somewhat agree", "Neither Agree nor disagree", "Somewhat disagree", "Totally disagree" };
            //    currentRow = excelApp.CreateInfoModule(currentRow, Constants.defaultFirstColumn, "ETA10", rightColTextArr);
            //    currentRow = excelApp.CreateResponseModule(currentRow, Constants.defaultFirstColumn, responseOptionsArr, QuestionType.Grid);
            //    endRow = currentRow + Constants.rowCountOffset;

            //    excelApp.BorderQuestionBlock(startRow, endRow, Constants.defaultFirstColumn);

            //    currentRow++;
            //}

            //currentRow = excelApp.CreateSectionHeader(currentRow++, Constants.defaultFirstColumn, "C", "A second section in the second sheet");

            //for (int i = 0; i < 3; i++)
            //{
            //    startRow = currentRow;
            //    // create info module. return current row after making info module
            //    string[] rightColTextArr = { "Age", "All respondents", "n/a", "Open field", "No", "All respondents", "Open numeric", "How many fingers am I holding?", "Please enter your answer below" };
            //    string[] responseOptionsArr = { Constants.ResponseCodes.GENERIC, Constants.SpecialResponses.PREFER_NO_ANSWER };
            //    currentRow = excelApp.CreateInfoModule(currentRow, Constants.defaultFirstColumn, "ETA10", rightColTextArr);
            //    currentRow = excelApp.CreateResponseModule(currentRow, Constants.defaultFirstColumn, responseOptionsArr, QuestionType.Grid);
            //    endRow = currentRow + Constants.rowCountOffset;

            //    excelApp.BorderQuestionBlock(startRow, endRow, Constants.defaultFirstColumn);

            //    currentRow++;
            //}

            //// open up the excel once all rows are constructed
            //excelApp.SetWindowVisibility(true);
            //excelApp.ShowFirstSheet(); // this only works after window is visible

            //excelApp.CleanUpExcelInteropObjs();
        }
    }
}
