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
    public class Response
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

        #region internal properties

        /// <summary>
        /// The response unique identifier
        /// <para>Requirements:</para>
        /// <list type="number">
        ///     <item>
        ///         <description>Must not be <c>null</c>.</description>
        ///     </item>
        ///     <item>
        ///         <description>Must be a unique <c>int</c> per response, across all responses and modules.</description>
        ///     </item>
        /// </list>
        /// </summary>
        public int RId { get; set; }


        /// <summary>
        /// The parent id. For a response, this is the id of the question which contains the response.
        /// </summary>
        public int PId { get; set; }

        public DateTime DateCreated { get; set; }
        public DateTime DateLastModified { get; set; }

        #endregion
    }
}
