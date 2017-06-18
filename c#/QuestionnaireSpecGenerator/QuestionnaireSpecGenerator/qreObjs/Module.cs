using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestionnaireSpecGenerator
{
    //TODO: When initializign a new qre object, all its fields need to be set to a DEFAULT VALUE

    /// <summary>
    ///  This class represents a single sheet (module) in the questionnaire.
    ///  This can be used for grouping sections with a unified purpose. Unlike <see cref="SectionSets"/> and <see cref="QuestionSets"/>,
    ///   <see cref="Modules"/> have a <c>locative</c> property, determining how modules are organized in the questionnaire.
    ///  <para>Modules are labeled with a <c>number</c> and a <c>title</c> (e.g. Module 1: Demographics), and may or
    ///   may not reflect the actual title of section(s) contained in the module.</para>
    /// </summary>
    public class Module
    {
        #region outward expressions

        /// <summary>
        /// The module number that shows in the sheet name (e.g. "Module 1").
        /// <para>Note: If <c>null</c>, then this attribute remains hidden from the sheet name.</para>
        /// </summary>
        public int MNum { get; set; }

        /// <summary>
        /// The module title that shows in the sheet name (e.g. "Module 1: Demographics").
        /// <para>Note: If <c>null</c>, then this attribute remains hidden from the sheet name.</para>
        /// </summary>
        public string MTitle { get; set; }

        /// <summary>
        /// The module description. This can be shown at the top of the sheet if desired (see <c>mShowDesc</c>).
        /// </summary>
        public string MDesc { get; set; }

        /// <summary>
        /// Determines whether to show the module description at the top of the sheet, beneath the module name.
        /// </summary>
        public bool MShowDesc { get; set; }

        /// <summary>
        /// The sections in the module, stored as a list.
        /// </summary>
        public List<Section> Sections { get; set; }

        #endregion

        #region locative properties  

        /// <summary>
        /// The sheet number. Determines the order in which sheets are shown in the questionnaire, from
        ///  left to right.
        /// <para>Requirements:</para>
        /// <list type="number">
        ///     <item>
        ///         <description>Must not be <c>null</c>.</description>
        ///     </item>
        ///     <item>
        ///         <description>Must be a unique <c>int</c> per module.</description>
        ///     </item>
        /// </list>
        /// </summary>
        public int SheetNum { get; set; }

        #endregion


        #region internal properties

        /// <summary>
        /// The module unique identifier.
        /// <para>Requirements:</para>
        /// <list type="number">
        ///     <item>
        ///         <description>Must not be <c>null</c>.</description>
        ///     </item>
        ///     <item>
        ///         <description>Must be a unique <c>int</c> per module.</description>
        ///     </item>
        /// </list>
        /// </summary>
        public int MId { get; set; }

        /// <summary>
        /// The module creation date and time
        /// </summary>
        public DateTime DateCreated { get; set; }

        /// <summary>
        /// The last time module was modified
        /// </summary>
        public DateTime DateLastModified { get; set; }

        #endregion

        #region Methods

        // TODO: Add methods

        public void AddSection(Section section)
        {
            Sections.Add(section);
        }

        public void RemoveSection(Section section)
        {
            if (Sections.Contains(section))
            {
                Sections.Remove(section);
            }
            else
            {
                throw new ArgumentOutOfRangeException("section", "The passed section does not exist in this module.");
            }
        }

        #endregion
    }
}
