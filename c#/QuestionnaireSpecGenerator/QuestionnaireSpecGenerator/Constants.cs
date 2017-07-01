using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestionnaireSpecGenerator
{
    public class Constants
    {
        internal class Colors
        {
            // #RGBs
            internal const string NO_FILL = "";
            internal const string LIGHT_GREEN = "#D6EAB0"; // 214, 234, 176
            internal const string DARK_GREEN = "#99CA3C"; // 153, 202, 60
            internal const string GREY = "#BFBFBF"; // 191, 191, 191
            internal const string BLACK = "#000000"; // 0, 0, 0
            internal const string WHITE = "#FFFFFF"; // 255, 255, 255
        }

        // currently assumes number of responses is 96 or less
        internal class ResponseCodes
        {
            internal const string GENERIC = "CODE";
            internal const string COLUMN = "COLUMN CODE";
            internal const string ROW = "ROW CODE";

            internal const int firstRow = 1;
            internal const int firstColumn = 1;
            internal const int all = 96;
            internal const int idk = 97;
            internal const int other = 98;
            internal const int none = 99;
        }

        internal class SpecialResponses
        {
            internal const string PREFER_NO_ANSWER = "Prefer not to answer";
            internal const string PREFER_NO_SAY = "Prefer not to say";
            internal const string NONE_OF_THE_ABOVE = "None of the above";
            internal const string OTHER = "Other";
            internal const string OTHER_SPECIFY = "Other (please specify)";
            internal const string DONT_KNOW = "Don't know";
            internal const string ALL_OF_THE_ABOVE = "All of the above";
        }

        internal static string[] flags =
        {
            "ANCHOR ON TOP",
            "ANCHOR ON BOTTOM",
            "MUTUALLY EXCLUSIVE",
            "TERM IF SELECTED"
        };

        // Column sizes
        internal const double leftColSize = 31;
        internal const double rightColSize = 100;

        // row heights
        internal const double defaultRowHeight = 14.25;
        internal const double defaultSectionRowHeight = 27.75;

        // font
        internal const string defaultFont = "Lucida Sans";
        internal const double defaultFontSize = 10;
        internal const double defaultSectionFontSize = 12;

        // firsts
        internal const int defaultFirstRow = 1;
        internal const int defaultFirstColumn = 1;
        internal const int rowCountOffset = -1;
        internal const int defaultColumnOffset = 1;

        // TODO: POTENTIALLY DEPRECATE- initial row counter values
        internal const int bufferHeight = 1;
        internal const int questionBlockInitHeight = 8 + bufferHeight;
        internal const int sectionInitHeight = 1 + bufferHeight;
    }

    // Respondent instruction strings based on
    internal class RespondentInstructionArrays
    {
        // ADC RI strings
        internal static string[] respInstADCStrings =
        {
            "Please select the appropriate symbol.",
            "Please type in your answer below.",
            "Please type in your ansewrs in the boxes below - one answer per box only.",
            "Please drag each logo into the circle, imagining the center is the closest you can " +
                "feel to the brand.",
            "Please select all brands you know of, even if you have already mentioned them in " +
                "previous questions.",
            "Drag and drop each brand onto the scale.",
            "Click on the scale value that best represents your opinion.",
            "Please drag each brand to the appropriate scale box to record your answer.",
            "Drag the image on each scale to where you think it fits for this brand." // TO BE CONTINUED
        };

        // Non-ADC RI strings
        internal static string[] respInstStrings =
        {
            "Enter your age in years in the box below.",

        };
    }

    // Arrays ordered based on RowType enum
    // TODO: Turn this into a dictionary
    internal class RowTypeArrays
    {
        // left column strings
        internal static string[] leftColStrings =
        {
            "Section Title", // placeholder, should not show
            "Question Number", // placeholder, should not show
            "Base Label/Definition",
            "Comment/Advice",
            "Programming Instructions",
            "Standard Fulcrum Question",
            "Routing Instructions",
            "Question Type",
            "Question Text",
            "Respondent instruction",
            "CODE",
            "COLUMN CODE",
            "ROW CODE",
            "Response Code", // placeholder, should not show
            "Custom text" // placeholder, should not show
        };

        // bold cells
        internal static TextFormats[] textFormats =
        {
            TextFormats.Bold,
            TextFormats.Bold,
            TextFormats.Normal,
            TextFormats.Normal,
            TextFormats.Normal,
            TextFormats.Normal,
            TextFormats.Normal,
            TextFormats.Normal,
            TextFormats.Bold,
            TextFormats.Italics,
            TextFormats.Bold,
            TextFormats.Bold,
            TextFormats.Bold,
            TextFormats.Normal,
            TextFormats.Normal
        };

        // row colors
        internal static string[] cellColors =
        {
            Constants.Colors.DARK_GREEN,
            Constants.Colors.NO_FILL,
            Constants.Colors.LIGHT_GREEN,
            Constants.Colors.GREY,
            Constants.Colors.LIGHT_GREEN,
            Constants.Colors.DARK_GREEN,
            Constants.Colors.LIGHT_GREEN,
            Constants.Colors.LIGHT_GREEN,
            Constants.Colors.NO_FILL,
            Constants.Colors.NO_FILL,
            Constants.Colors.NO_FILL,
            Constants.Colors.NO_FILL,
            Constants.Colors.NO_FILL,
            Constants.Colors.NO_FILL,
            Constants.Colors.NO_FILL
        };

    }

    internal enum RowType
    {
        SectionHeader = 0,
        QuestionTitle,
        BaseDefinition,
        Comments,
        ProgrammingInstruction,
        FulcrumQuestion,
        RoutingInstruction,
        QuestionType,
        QuestionText,
        RespondentInstruction,
        Code_Generic,
        Code_Column,
        Code_Row,
        Response,
        Custom,
        None = -4096
    }

    internal enum TextFormats
    {
        Normal = 0,
        Bold,
        Italics,
        Strikethrough
    }
}
