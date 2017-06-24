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
    public class Section
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
        public List<QuestionBlock> QuestionBlocks { get; set; }

        #endregion

        #region physical locative properties

        /// <summary>
        /// The section position. Determines the order in which sections are shown in the module, from
        ///  top to bottom.
        /// <para>Requirements:</para>
        /// <list type="number">
        ///     <item>
        ///         <description>Must not be <c>null</c>.</description>
        ///     </item>
        ///     <item>
        ///         <description>Must be a unique <c>int</c> per section within the same module.</description>
        ///     </item>
        /// </list>
        /// </summary>
        public int sPosition { get; set; } // TODO: Need to add to JSON

        /// <summary>
        /// The excel row of where the section begins. This is a <b>user-defined</b> value, representing the row number 
        ///     containing the section header. This determines where in the sheet the section gets drawn.
        /// <para>Requirements:</para>
        /// <list type="number">
        ///     <item>
        ///         <description>
        ///             Must be a unique <c>int</c> that can't conflict with any other row as defined by
        ///             other <see cref="Sections"/> or <see cref="QuestionBlocks"/>.
        ///         </description>
        ///     </item>
        /// </list>
        /// </summary>
        private int StartRow { get; set; }

        // TODO: POTENTIALLY DEPRECATE
        /// <summary>
        /// The excel row of where the <see cref="Section"/> ends. In the usual case, this is an <b>auto-defined</b> 
        ///     value, representing the row number 2 rows after the <c>endRow</c> of the last <see cref="QuestionBlock"/>
        ///     contained in the section. This is purely a reference value, which does not affect where the 
        ///     section gets drawn on the sheet (see <see cref="StartRow"/>).
        /// <para>Requirements:</para>
        /// <list type="number">
        ///     <item>
        ///         <description>
        ///             Must be a unique <c>int</c> that can't conflict with any other row as defined by
        ///             other <see cref="Sections"/> or <see cref="QuestionBlocks"/>.
        ///         </description>
        ///     </item>
        /// </list>
        /// </summary>
        public int EndRow { get; set; }

        /// <summary>
        /// Keeps track of the number of rows the section uses. This is contingent on the size of each
        /// <see cref="QuestionBlock"/> contained in the section. Also includes an extra row for bottom padding. 
        /// <para>
        /// <b>Note: </b>Unless initializing, do NOT directly modify this counter.
        /// </para>
        /// </summary>
        private int rowCounter { get; set; } // TODO: Need to add to JSON

        #endregion

        #region internal properties

        /// <summary>
        /// The section unique identifier
        /// <para>Requirements:</para>
        /// <list type="number">
        ///     <item>
        ///         <description>Must not be <c>null</c>.</description>
        ///     </item>
        ///     <item>
        ///         <description>Must be a unique <c>int</c> per section, across all modules.</description>
        ///     </item>
        /// </list>
        /// </summary>
        public int SId { get; set; }

        /// <summary>
        /// The parent id. For a section, this is the id of the module which contains the section.
        /// </summary>
        public int PId { get; set; }

        /// <summary>
        /// The <see cref="Section"/> creation date and time
        /// </summary>
        public DateTime DateCreated { get; set; }

        /// <summary>
        /// The last time <see cref="Section"/> was modified
        /// </summary>
        public DateTime DateLastModified { get; set; }

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
        /// Initializes a new instance of the <see cref="Section"/> class and adds it to the <see cref="DataContainer"/>.
        /// </summary>
        /// <param name="container">The main data container (<b>required</b>).</param>
        /// <param name="pId">The parent identifier (<b>required</b>).</param>
        /// <param name="sLetter">The section letter.</param>
        /// <param name="sTitle">The section title.</param>
        /// <param name="questionBlocks">The question blocks contained in the section.</param>
        public Section(DataContainer container, int pId, int position, string sLetter = "XYZ", string sTitle = "Section Name",
            string sDesc = "The section description", bool sShowDesc = false, List<QuestionBlock> questionBlocks = null)
        {
            DateCreated = DateTime.Now;

            PId = pId;
            SLetter = sLetter;
            STitle = sTitle;
            SDesc = sDesc;
            SShowDesc = sShowDesc;
            sPosition = position;

            rowCounter = Constants.sectionInitHeight;
            if (sShowDesc) rowCounter++;

            // TODO: Add method that determines the physical locative properties (in module class)
            // Need to determine startRow here, based on the start position


            SId = Toolbox.GenerateRandomId(container, QreObjTypes.Section);

            // Add the question blocks (also determine start rows here)
            if (questionBlocks != null)
            {
                foreach (QuestionBlock question in questionBlocks)
                {
                    AddQuestion(question);
                }
            }
            container.AddSection(this);
            UpdateDate();
        }

        /// <summary>
        /// Adds the question to the list of questions for the <see cref="Section"/>. Sets each
        /// <see cref="QuestionBlock.PId"/> to the current <see cref="Section.SId"/>. Also 
        /// updates the <see cref="rowCounter"/> as well as <see cref="DateLastModified"/>.
        /// <para>
        /// Note, when creating the question to be added, no need to put down a Parent ID, since that is
        /// done here!
        /// </para>
        /// </summary>
        /// <param name="question">The question.</param>
        public void AddQuestion(QuestionBlock question)
        {
            question.PId = SId;
            // LEFT OFF TODO: Set start row here before adding (recently added s and q position variables for ordering.
            // This can somehow depend on that ordering

            // TODO: When updating the row counter, need to also update the start rows of all questions afterwards, AND need




            QuestionBlocks.Add(question);
            rowCounter += question.GetNumRows();
            UpdateDate();
        }

        /// <summary>
        /// Removes the question, and updates the <see cref="rowCounter"/> as well as <see cref="DateLastModified"/>.
        /// </summary>
        /// <param name="question">The question.</param>
        /// <exception cref="ArgumentOutOfRangeException">question - The passed question does not exist in this section.</exception>
        public void RemoveQuestion(QuestionBlock question)
        {
            if (QuestionBlocks.Contains(question))
            {
                QuestionBlocks.Remove(question);
                rowCounter -= question.GetNumRows();
                UpdateDate();
            }
            else
            {
                throw new ArgumentOutOfRangeException("question", "The passed question does not exist in this section.");
            }
        }

        /// <summary>
        /// Gets the position of the section, relative to other sections in the same module.
        /// </summary>
        /// <returns></returns>
        public int GetPosition()
        {
            return sPosition;
        }

        /// <summary>
        /// Sets the position of the section relative to other sections in the same module.
        /// Positions must be zero-indexed, with 0 denoting the first position.
        /// </summary>
        /// <param name="position">The position to set.</param>
        public void SetPosition(int position)
        {
            sPosition = position;
            UpdateDate();
        }

        public int GetStartRow()
        {
            return StartRow;
        }

        public void SetStartRow(int startRow)
        {
            StartRow = startRow;
            UpdateDate();
        }

        public int GetRowCounter()
        {
            return rowCounter;
        }

        /// <summary>
        /// Updates <see cref="DateLastModified"/>. Needs to be called in methods that update <see cref="Section"/> elements.
        /// </summary>
        private void UpdateDate()
        {
            DateLastModified = DateTime.Now;

            // TODO: Whenever this method is called, need to also compile a log of changes for the changelog. PRE and POST states
        }

        #endregion
    }
}
