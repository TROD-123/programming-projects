using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestionnaireSpecGenerator
{
    /// <summary>
    /// The point of these Programming Instruction flags is to save users time from writing long,
    /// redundant and convoluted programming instructions in the spec. These flags are intended to
    /// auto-generate these PIs - users would simply have to pick and choose their relevant flags.
    /// <para>
    /// Note: These are used for ADC questions. Please see <see cref="ProgFlagsNonADC"/> for Non-ADC
    /// question flags. For a question block, it is required that either <see cref="ProgFlagsNonADC"/> or
    /// <see cref="ProgFlagsADC"/> is not <c>null</c>.
    /// </para>
    /// <para><b>Use:</b> These flags should be stored in a list.</para>
    /// </summary>
    public enum ProgFlagsNonADC
    {
        RadioButtons = 0, // aka Single code. if one selection type selected, disable the others
        CheckBoxes, // aka Multi code.
        NumOE, // if one OE selected, disable the others
        BrandOE,
        FullOE,
        HorizontalScale,
        Grid,
        ShowBrandLogos,




        Custom = 4096 // user-defined instructions to append to the auto-generated PI
    }
}
