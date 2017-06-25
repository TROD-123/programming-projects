using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestionnaireSpecGenerator
{
    /// <summary>
    ///  A class for a single response and its attributes.
    /// </summary>
    public class Response : QreObjBase
    {
        #region outward expressions

        /// <summary>
        /// The coded value of the response.
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// The text of the response as it appears to the respondent (e.g. "Male").
        /// </summary>
        public string RText { get; set; }

        /// <summary>
        /// The list of response flags that specify additional attributes to the response.
        /// </summary>
        public List<ResponseFlags> RFlags { get; set; }

        #endregion

        public override void UpdateDate()
        {
            DateModified = DateTime.Now;

            // TODO: Need to implement method that will store the changes user makes to the object
        }
    }
}
