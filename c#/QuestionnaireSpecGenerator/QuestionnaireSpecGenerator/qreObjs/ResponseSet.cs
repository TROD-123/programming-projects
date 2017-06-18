using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestionnaireSpecGenerator
{
    /// <summary>
    ///  This class represents a list of possible responses.
    ///  This can be used for a specific question, or as a reusable list for more standardized questions.
    /// </summary>
    public class ResponseSet
    {
        // outward expressions
        string rSetTitle;
        string rSetDesc;
        private List<Response> responses;

        // internal properties
        int rSetId;
        DateTime dateCreated;
        DateTime dateLastModified;
    }
}
