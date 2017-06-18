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
    /// <para><b>Use:</b> These flags should be stored in a list</para>
    /// </summary>
    // TODO: "Advanced Flags" to define the specific configuration of ADC
    public enum ProgFlagsADC
    {
        Gender = 0, // overall hw, individual hw, font size
        SingleOpenTextBox, // overall hw, default text, font size, color
        MultipleOpenTextBox, // overall hw, default text for each box, # of boxes, font size, color
        HeartMatrix, // overall hw, hw of circle matrix, color, gradient, and # of rings
        BrandListLogoSelect,
        BrandListTextSelect,
        HorizontalScaleMultipleBrandLogos,
        HorizontalScaleSingleStatement,
        VerticalScaleDragDropLogos,
        MultipleSliderMultipleStatement // TO BE CONTINUED
    }
}
