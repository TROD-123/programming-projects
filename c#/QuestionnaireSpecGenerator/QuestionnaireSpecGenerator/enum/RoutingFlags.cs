using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestionnaireSpecGenerator
{
    /// <summary>
    /// The point of these Routing Instruction flags is to save users time from writing long,
    /// redundant and convoluted routing instructions in the spec. These flags are intended to
    /// auto-generate these RIs - these flags are chosen automatically 
    /// </summary>
    public enum RoutingFlags
    {
        NextQuestion = 0,
        SkipTo,
        EndSurvey
    }
}
