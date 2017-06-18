using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestionnaireSpecGenerator
{
    /// <summary>
    /// Classification of all the <see cref="Questionnaire"/> objects by type.
    /// </summary>
    public enum QreObjTypes
    {
        Questionnaire = 0,
        Module,
        Section,
        QuestionBlock,
        Response
    }
}
