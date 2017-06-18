using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestionnaireSpecGenerator
{
    /// <summary>
    ///  This class represents a list of sections.
    ///  This can be used for grouping sections together with a unified purpose, or to classify sections
    ///   as belonging to a specific module in the questionnaire.
    ///  This is purely for a user's own organizing purposes and does not impact locations of sections
    ///   in the questionnaire.
    /// </summary>
    public class SectionSet
    {
        // outward expressions
        string sSetTitle;
        string sSetDesc;
        List<Section> sections;

        // internal properties
        int sSetId;
        DateTime dateCreated;
        DateTime dateLastModified;
    }
}
