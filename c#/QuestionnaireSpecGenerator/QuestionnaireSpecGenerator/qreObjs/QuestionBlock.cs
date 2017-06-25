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
    public class QuestionBlock : QuestionnaireObject<Response>
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
        //public List<Response> Responses { get; set; }


        #endregion

        #region methods

        /// <summary>
        /// Prevents a default instance of the <see cref="QuestionBlock"/> class from being created. Used for deserialization
        /// by <see cref="JsonHandler"/>.
        /// </summary>
        private QuestionBlock()
        {
            // NOTE: Beware to NOT call UpdateDate() when merely DESERIALIZING.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="QuestionBlock"/> class and adds it to the <see cref="DataContainer"/>.
        /// </summary>
        /// <param name="container">The main data container (<b>required</b>).</param>
        /// <param name="pId">The parent identifier (<b>required</b>).</param>
        /// <param name="qNum">The question number.</param>
        /// <param name="qTitle">The question title.</param>
        /// <param name="baseLabel">The base label.</param>
        /// <param name="baseDef">The base definition.</param>
        /// <param name="comments">The comments.</param>
        /// <param name="progFlagsNonADC">The list of Non-ADC programming flags. Used to set the <see cref="ProgInst"/> string.</param>
        /// <param name="progFlagsADC">The list of ADC programming flags. Used to set the <see cref="ProgInst"/> string.</param>
        /// <param name="rFlag">The routing flag. Used to set the <see cref="RoutInst"/> string.</param>
        /// <param name="qTypeInt">The question type. Used to set the <see cref="QType"/> string.</param>
        /// <param name="qText">The question text.</param>
        /// <param name="respInst">The respondent instruction.</param>
        /// <param name="responses">The list of responses.</param>
        public QuestionBlock(DataContainer container, int pId, string qNum = "QNUM", string qTitle = "Question title",
            string baseLabel = "Base label", string baseDef = "Base definition", string comments = "Comments",
            List<ProgFlagsNonADC> progFlagsNonADC = null, List<ProgFlagsADC> progFlagsADC = null, 
            RoutingFlags rFlag = RoutingFlags.NextQuestion, QuestionType qTypeInt = QuestionType.SingleCode,
            string qText = "Question text", string respInst = "Respondent instruction", List<Response> responses = null)
        {
            DateCreated = DateTime.Now;

            ParentId = pId;
            QNum = qNum;
            QTitle = qTitle;
            BaseLabel = baseLabel;
            BaseDef = baseDef;
            Comments = comments;
            ProgFlagsNonADC = progFlagsNonADC;
            ProgFlagsADC = progFlagsADC;
            RFlag = rFlag;
            QTypeInt = qTypeInt;
            QText = qText;
            RespInst = respInst;

            SelfId = Toolbox.GenerateRandomId(container, QreObjTypes.QuestionBlock);

            // Add the responses
            if (responses != null)
            {
                foreach (Response response in responses)
                {
                    AddChild(response);
                }
            }
            container.AddQuestion(this);
            UpdateDate();
        }

        // TODO: REMOVE FROM HERE ONCE IMPLEMENTED IN BASE CLASS
        //public void ChangeParent(int newPId)
        //{
        //    // Need to get the current section and the target sections, and modify the question lists from there
        //    int oldPId = ParentId;

        //    //Section oldParent = DataContainer.GetSectionById(oldPId);
        //    //Section newParent = DataContainer.GetSectionById(newPId);

        //    //oldParent.RemoveQuestion(this);
    
        //    // will also need to reflect the changed parent id here
        //    ParentId = newPId;
        //    //newParent.AddQuestion(this);
        //}   

        #endregion
    }
}
