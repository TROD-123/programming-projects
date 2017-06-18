using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestionnaireSpecGenerator
{
    /// <summary>
    ///  This class represents a single question, containing its attributes as well as its list
    ///  of possible responses.
    /// </summary>
    public class QuestionBlock
    {
        #region outward expressions

        /// <summary>
        /// The question number. 
        /// <para>This field is <b>required</b> (can't be <c>null</c>).</para>
        /// </summary>
        public string QNum { get; set; } // required

        /// <summary>
        /// The title of the question.
        /// <para>This field is <b>required</b> (can't be <c>null</c>).</para>
        /// </summary>
        public string QTitle { get; set; } // required

        /// <summary>
        /// The base label, i.e. the user-friendly label stating for whom this question is asked 
        ///  (e.g. "All respondents", or "Male respondents"). 
        /// <para>This field is <b>required</b> (can't be <c>null</c>).</para>
        /// <para>
        /// Note: If there is a special <c>baseLabel</c>, then a <c>baseDef</c> is appended to the end of 
        ///  <c>baseLabel</c> in the <c>baseLabel</c> field in the questionnaire spec (e.g. "Male respondents: 
        ///  Q1 = 1 ['Males']").
        /// </para>
        /// </summary>
        public string BaseLabel { get; set; } // required

        /// <summary>
        /// The programmer base definition, i.e. to whom this question is asked. This is how <c>baseLabel</c> is
        /// defined for the programmers (e.g. "Q1 = 1 ['Males']"). This definition is appended to the <c>baseLabel</c>
        /// field in the questionnaire spec (e.g. "Male respondents: Q1 = 1 ['Males']").
        /// <para>This field is <b>optional</b> (can be <c>null</c>).</para>
        /// <para>Note: If there is no special <c>baseLabel</c>, then this can be left <c>null</c>.</para>
        /// </summary>
        public string BaseDef { get; set; } // optional

        /// <summary>
        /// Relevant comments or advice to show in the <c>comments</c> field in the questionnaire spec.
        /// <para>
        /// This field is <b>optional</b> (can be <c>null</c>). If <c>null</c> then this field is hidden from
        /// the questionnaire spec.
        /// </para>
        /// </summary>
        public string Comments { get; set; } // optional

        /// <summary>
        /// An auto-generated programming instruction string. This describes how the question should be shown to the 
        /// respondent, including:
        /// <list type="bullet">
        ///     <item><description>How statements should be displayed</description></item>
        ///     <item><description>Use of a text box, and its attributes</description></item>
        ///     <item><description>Termination instructions</description></item>
        ///     <item><description>Response coding instructions</description></item>
        ///     <item><description>Piping instructions</description></item>
        /// </list>
        /// <para>
        /// Because there can be several components to a PI, each of these properties are abstracted via <see cref="ProgFlagsNonADC"/>
        /// which are used to auto-generate this programming instruction string.
        /// </para>
        /// </summary>
        public string ProgInst { get; set; } // since this string is auto-generated, consider NOT including this in the JSON in case users modify it

        /// <summary>
        /// The list of programming flags for Non ADC questions. This list generates the <see cref="ProgInst"/> string.
        /// <para>
        /// Note: If this question is ADC, this must be set to <c>null</c>. At least one of <see cref="ProgFlagsNonADC"/> or
        /// <see cref="ProgFlagsADC"/> must not be <c>null.</c>
        /// </para>
        /// </summary>
        public List<ProgFlagsNonADC> ProgFlagsNonADC { get; set; }

        /// <summary>
        /// The list of programming flags for ADC questions. This list generates the <see cref="ProgInst"/> string.
        /// <para>
        /// Note: If this question is not ADC, this must be set to <c>null</c>. At least one of <see cref="ProgFlagsNonADC"/> or
        /// <see cref="ProgFlagsADC"/> must not be <c>null.</c>
        /// </para>
        /// </summary>
        public List<ProgFlagsADC> ProgFlagsADC { get; set; }

        /// <summary>
        /// An auto-generated routing instruction string. This describes which question or actions should follow after
        /// the current question.
        /// </summary>
        public string RoutInst { get; set; } // since this string is auto-generated, consider NOT including this in the JSON in case users modify it

        /// <summary>
        /// The routing flag used to generate the <see cref="RoutInst"/> string.
        /// </summary>
        public RoutingFlags RFlag { get; set; }

        /// <summary>
        /// An auto-generated string detailing the <see cref="QuestionType"/>, which translates the <see cref="QuestionType"/>
        /// flag defined in <see cref="QTypeInt"/>.
        /// </summary>
        public string QType { get; set; } // since this string is auto-generated, consider NOT including this in the JSON in case users modify it

        /// <summary>
        /// The question type flag used to generate the <see cref="QType"/> string.
        /// </summary>
        public QuestionType QTypeInt { get; set; }

        /// <summary>
        /// The question text that is shown to the respondent (e.g. "What is your gender?"). This is typically <b>user-defined</b>.
        /// </summary>
        public string QText { get; set; }

        /// <summary>
        /// The instruction for the respondent (e.g. "Please select one answer below."). This is typically chosen from a bank
        /// of respondent intsructions, but can also be user-defined.
        /// </summary>
        public string RespInst { get; set; }

        /// <summary>
        /// The list of response options for the respondent to select from, depending on the question. Can be <c>null</c> if
        /// the <see cref="QType"/> is <c>BreakScreen</c>, <c>TextField</c> or <c>Marker</c>.
        /// </summary>
        public List<Response> Responses { get; set; }

        #endregion

        #region locative properties

        /// <summary>
        /// The excel row of where the <see cref="QuestionBlock"/> begins. This is a <b>user-defined</b> value, representing 
        ///  the row number containing the question number and title. This determines where in the sheet the 
        ///  <see cref="QuestionBlock"/> gets drawn.
        /// <para>Requirements:</para>
        /// <list type="number">
        ///     <item>
        ///         <description>
        ///             Must be a unique <c>int</c> that can't conflict with any other row as defined by
        ///             other <see cref="Sections"/> or <see cref="QuestionBlocks"/> within the same <see cref="Section"/>.
        ///         </description>
        ///     </item>
        /// </list>
        /// </summary>
        public int StartRow { get; set; }

        /// <summary>
        /// The excel row of where the <see cref="QuestionBlock"/> ends. In the usual case, this is an <b>auto-defined</b> 
        ///     value, representing the last row of the <see cref="QuestionBlock"/>. This is purely a reference value, 
        ///     which does not affect where the <see cref="QuestionBlock"/> gets drawn on the sheet (see <see cref="StartRow"/>).
        /// <para>Requirements:</para>
        /// <list type="number">
        ///     <item>
        ///         <description>
        ///             Must be a unique <c>int</c> that can't conflict with any other row as defined by
        ///             other <see cref="Sections"/> or <see cref="QuestionBlocks"/> within the same <see cref="Section"/>.
        ///         </description>
        ///     </item>
        /// </list>
        /// </summary>
        public int EndRow { get; set; }

        #endregion

        #region internal properties

        /// <summary>
        /// The question unique identifier
        /// <para>Requirements:</para>
        /// <list type="number">
        ///     <item>
        ///         <description>Must not be <c>null</c>.</description>
        ///     </item>
        ///     <item>
        ///         <description>Must be a unique <c>int</c> per question, across all modules.</description>
        ///     </item>
        /// </list>
        /// </summary>
        public int QId { get; set; }

        /// <summary>
        /// The parent id. For a question, this is the id of the section which contains the question.
        /// </summary>
        public int PId { get; set; }

        /// <summary>
        /// The <see cref="QuestionBlock"/> creation date and time
        /// </summary>
        public DateTime DateCreated { get; set; }

        /// <summary>
        /// The last time <see cref="QuestionBlock"/> was modified
        /// </summary>
        public DateTime DateLastModified { get; set; }

        #endregion

        #region methods

        public void AddResponse(Response response)
        {
            // Whatever the response id is currently set to, set the id to the current question block id
            response.PId = QId;
            Responses.Add(response);
        }

        public void RemoveResponse(Response response)
        {
            if (Responses.Contains(response))
            {
                Responses.Remove(response);
            }
            else
            {
                throw new ArgumentOutOfRangeException("response", "The passed response does not exist in this question.");
            }
        }

        public void ChangeParent(int newPId)
        {
            // Need to get the current section and the target sections, and modify the question lists from there
            int oldPId = PId;

            Section oldParent = DataContainer.GetSectionById(oldPId);
            Section newParent = DataContainer.GetSectionById(newPId);

            oldParent.RemoveQuestion(this);
    
            // will also need to reflect the changed parent id here
            PId = newPId;
            newParent.AddQuestion(this);
        }

        // NOTE: Since the DataContainer class houses all qre objects, we should give this class the cross-object functionality
        // e.g. changing parents, setting parents. So that we don't have to define redundant methods across each of the classes


        #endregion
    }
}
