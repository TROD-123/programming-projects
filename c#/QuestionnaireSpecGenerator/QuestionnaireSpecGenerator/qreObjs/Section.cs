using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestionnaireSpecGenerator
{
    /// <summary>
    ///  This class represents a single section in the questionnaire.
    ///  This can contain one or multiple <c>QuestionBlocks</c>.
    ///  <para>Sections are labeled with an optional <c>letter</c> and an optional <c>title</c> 
    ///   (e.g. Section A: Demographics)</para>
    /// </summary>
    public class Section : QuestionnaireObject<QuestionBlock>
    {
        #region outward expressions

        /// <summary>
        /// The section letter that shows in the section name (e.g. "Section A").
        /// <para>Notes:</para>
        /// <list type="number">
        ///     <item>
        ///         <description>
        ///             If <c>null</c>, then this attribute remains hidden from the section name
        ///                 (e.g. if only <c>sTitle</c> is defined, then the section name is shown as
        ///                 "Demographics").
        ///         </description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             At least one of <c>sLetter</c> or <c>sTitle</c> <b>must</b> not be <c>null</c>.
        ///         </description>
        ///     </item>
        /// </list>
        /// </summary>
        public string SLetter { get; set; }

        /// <summary>
        /// The section title that shows in the section name (e.g. "Section A: Demographics").
        /// <para>Notes:</para>
        /// <list type="number">
        ///     <item>
        ///         <description>
        ///             If <c>null</c>, then this attribute remains hidden from the section name
        ///                 (e.g. if only <c>sLetter</c> is defined, then the section name is shown as
        ///                 "Section A").
        ///         </description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             At least one of <c>sLetter</c> or <c>sTitle</c> <b>must</b> not be <c>null</c>.
        ///         </description>
        ///     </item>
        /// </list>
        /// </summary>
        public string STitle { get; set; }

        /// <summary>
        /// The description that shows below the section name, if desired (see <see cref="SShowDesc"/>).
        /// </summary>
        public string SDesc { get; set; } // TODO: Add to JSON

        /// <summary>
        /// Determines whether to show the section description at the top of the section, beneath the section name.
        /// </summary>
        public bool SShowDesc { get; set; } // TODO: Add to JON


        /// <summary>
        /// The question blocks in the section, stored as a list.
        /// </summary>
        //public List<QuestionBlock> QuestionBlocks { get; set; }

        #endregion

        #region methods

        /// <summary>
        /// Prevents a default instance of the <see cref="Section"/> class from being created. Used for deserialization by
        /// <see cref="JsonHandler"/>.
        /// </summary>
        private Section()
        {


        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Section" /> class.
        /// </summary>
        /// <param name="pId">The parent identifier.</param>
        /// <param name="sLetter">The section letter.</param>
        /// <param name="sTitle">The section title.</param>
        /// <param name="sDesc">The s desc.</param>
        /// <param name="sShowDesc">if set to <c>true</c> [s show desc].</param>
        /// <param name="questionBlocks">The question blocks contained in the section.</param>
        public Section(int pId, string sLetter, string sTitle,
            string sDesc, bool sShowDesc, List<QuestionBlock> questionBlocks)
        {
            DateCreated = DateTime.Now;

            ParentId = pId;
            SLetter = sLetter;
            STitle = sTitle;
            SDesc = sDesc;
            SShowDesc = sShowDesc;

            Children = new List<QuestionBlock>();


            // Add the question blocks (also determine start rows here)
            if (questionBlocks != null)
            {
                foreach (QuestionBlock question in questionBlocks)
                {
                    AddChild(question);
                }
            }
            UpdateDate();
        }

        // TODO: Add methods

        #endregion

        #region deprecated

        ///// <summary>
        ///// Adds the question to the list of questions for the <see cref="Section"/>. Sets each
        ///// <see cref="QuestionBlock.ParentId"/> to the current <see cref="Section.SId"/>. Also 
        ///// updates the <see cref="rowCounter"/> as well as <see cref="DateLastModified"/>.
        ///// <para>
        ///// Note, when creating the question to be added, no need to put down a Parent ID, since that is
        ///// done here!
        ///// </para>
        ///// </summary>
        ///// <param name="question">The question.</param>
        //public void AddQuestion(QuestionBlock question)
        //{
        //    question.ParentId = SelfId;
        //    // LEFT OFF TODO: Set start row here before adding (recently added s and q position variables for ordering.
        //    // This can somehow depend on that ordering

        //    // TODO: When updating the row counter, need to also update the start rows of all questions afterwards, AND need




        //    Children.Add(question);
        //    UpdateDate();
        //}

        ///// <summary>
        ///// Removes the question, and updates the <see cref="rowCounter"/> as well as <see cref="DateLastModified"/>.
        ///// </summary>
        ///// <param name="question">The question.</param>
        ///// <exception cref="ArgumentOutOfRangeException">question - The passed question does not exist in this section.</exception>
        //public void RemoveQuestion(QuestionBlock question)
        //{
        //    if (QuestionBlocks.Contains(question))
        //    {
        //        QuestionBlocks.Remove(question);
        //        UpdateDate();
        //    }
        //    else
        //    {
        //        throw new ArgumentOutOfRangeException("question", "The passed question does not exist in this section.");
        //    }
        //}

        ///// <summary>
        ///// Gets the position of the section, relative to other sections in the same module.
        ///// </summary>
        ///// <returns></returns>
        //public int GetPosition()
        //{
        //    return Position;
        //}

        ///// <summary>
        ///// Sets the position of the section relative to other sections in the same module.
        ///// Positions must be zero-indexed, with 0 denoting the first position.
        ///// </summary>
        ///// <param name="position">The position to set.</param>
        //public void SetPosition(int position)
        //{
        //    Position = position;
        //    UpdateDate();
        //}

        ///// <summary>
        ///// Updates <see cref="DateLastModified"/>. Needs to be called in methods that update <see cref="Section"/> elements.
        ///// </summary>
        //private void UpdateDate()
        //{
        //    DateLastModified = DateTime.Now;

        //    // TODO: Whenever this method is called, need to also compile a log of changes for the changelog. PRE and POST states
        //}

        #endregion
    }
}
