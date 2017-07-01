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

        /// <summary>
        /// Prevents a default instance of the <see cref="Response"/> class from being created. Used for deserialization
        /// by <see cref="JsonHandler"/>.
        /// </summary>
        private Response()
        {
            // NOTE: Beware to NOT call UpdateDate() when merely DESERIALIZING.
        }

        public Response(int pId, int code, string rText, List<ResponseFlags> rFlags)
        {
            DateCreated = DateTime.Now;

            ParentId = pId;
            Code = code;
            RText = rText;

            RFlags = new List<ResponseFlags>();

            // Add the flags
            if (rFlags != null)
            {
                foreach (ResponseFlags flag in rFlags)
                {
                    AddFlag(flag);
                }
            }
            UpdateDate();
        }

        private void AddFlag(ResponseFlags flag)
        {

        }

        public override void UpdateDate()
        {
            DateModified = DateTime.Now;

            // TODO: Need to implement method that will store the changes user makes to the object
        }
    }
}
