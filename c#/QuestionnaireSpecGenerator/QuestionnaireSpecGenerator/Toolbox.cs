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

        /// <summary>
        /// Generates the random identifier code for a given questionnaire item type. Looks through an internalized
        /// list of ids currently taken for a provided type, and outputs an unused id for that type.
        /// <para>
        /// <b>Definition:</b> A non-zero, positive <see cref="int"/> with an upper limit of <see cref="int.MaxValue"/>.
        /// </para>
        /// </summary>
        /// <param name="container">The container holding the master <see cref="Questionnaire"/> object.</param>
        /// <param name="type">
        /// The type of the object requesting a unique identifier. Must be one of:
        /// <list type="number">
        /// <item><description><see cref="QreObjTypes.Module"/></description></item>
        /// <item><description><see cref="QreObjTypes.Section"/></description></item>
        /// <item><description><see cref="QreObjTypes.QuestionBlock"/></description></item>
        /// </list>
        /// </param>
        /// <returns>A unique id for the given <c>Type</c>. Returns <c>-1</c> if an exception occurs.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// type - Incompatible type passed. Only accepts Module, Section, and Questionblock qre object types.
        /// </exception>
        internal static int GenerateRandomId(DataContainer container, QreObjTypes type)
        {
            List<int> usedIds;
            try
            {
                usedIds = container.GetIds(type);
                int i = 1;
                while (true)
                {
                    if (usedIds.BinarySearch(i) < 0)
                    {
                        return i;
                    }
                    else
                    {
                        i++;
                    }
                }

            }
            catch (ArgumentOutOfRangeException e)
            {
                Console.WriteLine("GenerateRandomId threw an exception: " + e + " Returning -1...");
                return -1;
            }
        }
    }
}
