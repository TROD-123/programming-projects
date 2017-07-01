using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestionnaireSpecGenerator
{
    /// <summary>
    /// Set of helper functions
    /// </summary>
    internal class Toolbox
    {
        /// <summary>
        /// Gets the name of the excel column. 
        /// From https://stackoverflow.com/questions/181596/how-to-convert-a-column-number-eg-127-into-an-excel-column-eg-aa
        /// </summary>
        /// <param name="colNum">The column number.</param>
        /// <returns>The letter name of the excel column.</returns>
        internal static string GetExcelColumnName(int colNum)
        {
            int dividend = colNum;
            string colName = String.Empty;
            int modulo;

            while (dividend > 0)
            {
                modulo = (dividend - 1) % 26;
                colName = Convert.ToChar(65 + modulo).ToString() + colName;
                dividend = (int)((dividend - modulo) / 26);
            }

            return colName;
        }

        internal static void UpdateDate<T>(T obj) where T : Section
        {
            
        }
    }
}
