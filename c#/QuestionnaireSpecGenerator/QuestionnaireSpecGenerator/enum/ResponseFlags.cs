using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestionnaireSpecGenerator
{
    /// <summary>
    /// Display and behavioral attributes for each individual response.
    /// </summary>
    public enum ResponseFlags
    {
        Generic = 0, // can't be selcted with any of groups 1-3 below

        // Group 1: Grid flags. Choose one
        RowResponse = 10,
        ColumnResponse,

        // Group 2: Response type flags. Choose one
        PreferNo = 20,
        Other,
        NoneOfTheAbove,

        // Group 3: Anchor flags. Choose one
        AnchorBottom = 30,
        AnchorTop,
        AnchorLeft,
        AnchorCenter,
        AnchorRight,

        // Group 4: Miscellaneous flags
        MutuallyExclusive = 100,
        Terminate = 200
    }
}

