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
        /// The question blocks in the section, stored as a list.
        /// </summary>
        public List<QuestionBlock> QuestionBlocks { get; set; }

        #endregion

        #region physical locative properties

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
        public int StartRow { get; set; }

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

        public void AddQuestion(QuestionBlock question)
        {
            QuestionBlocks.Add(question);
        }

        public void RemoveQuestion(QuestionBlock question)
        {
            if (QuestionBlocks.Contains(question))
            {
                QuestionBlocks.Remove(question);
            }
            else
            {
                throw new ArgumentOutOfRangeException("question", "The passed question does not exist in this section.");
            }
        }

        #endregion
    }
}
