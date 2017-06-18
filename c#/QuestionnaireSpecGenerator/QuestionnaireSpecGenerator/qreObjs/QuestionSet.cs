using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestionnaireSpecGenerator
{
    /// <summary>
    ///  This class represents a list of questions.
    ///  This can be used for grouping questions together with a unified purpose, or to classify questions
    ///   as belonging to a specific section in the questionnaire.
    ///  This is purely for a user's own organizing purposes and does not impact locations of questions
    ///   in the questionnaire.
    /// </summary>
    public class QuestionSet
    {
        // outward expressions
        string qSetTitle;
        string qSetDesc;
        List<QuestionBlock> questions;

        // internal properties
        int qSetId;
        DateTime dateCreated;
        DateTime dateLastModified;
    }
}
