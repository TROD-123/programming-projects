using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestionnaireSpecGenerator
{
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

        // TODO: Tracker to keep track of startRow and endRow properties of sections (be wary of section deletes, 
        // question updates, response adds, etc.

        

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

        /// <summary>
        /// Prevents a default instance of the <see cref="Module"/> class from being created. Used for deserialization by
        /// <see cref="JsonHandler"/>.
        /// </summary>
        private Module()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Module" /> class and adds it to the <see cref="DataContainer"/>.
        /// </summary>
        /// <param name="container">The main data container (<b>required</b>).</param>
        /// <param name="mNum">The module number.</param>
        /// <param name="mTitle">The module title.</param>
        /// <param name="mDesc">The module description.</param>
        /// <param name="mShowDesc">If set to <c>true</c>, show the module description.</param>
        /// <param name="sections">The sections of the module.</param>
        public Module(DataContainer container, int mNum = 0, string mTitle = "Module name", 
            string mDesc = "Module description", bool mShowDesc = false, List<Section> sections = null)
        {
            DateCreated = DateTime.Now;

            MNum = mNum;
            MTitle = mTitle;
            MDesc = mDesc;
            MShowDesc = mShowDesc;

            SheetNum = container.GetNextSheetNum();

            MId = Toolbox.GenerateRandomId(container, QreObjTypes.Module);

            // Add the sections (also determine start rows here)
            if (sections != null)
            {
                foreach (Section section in sections)
                {
                    AddSection(section);
                }
            }

            container.AddModule(this);
            UpdateDate();
        }

        // TODO: Add methods

        /// <summary>
        /// Adds the section to the list of sections for the <see cref="Module"/>. Sets each
        /// <see cref="Section.SId"/> to the current <see cref="Module.MId"/>. Also updates the
        /// <see cref="DateLastModified"/>.
        /// <para>
        /// Note, when creating the section to be added, no need to put down a Parent ID, since that is
        /// done here!
        /// </para>
        /// </summary>
        /// <param name="section">The section.</param>
        public void AddSection(Section section)
        {
            // Set the section PID
            section.SId = MId;

            // Get the section position
            int posNewSection = section.GetPosition();

            // First, increment positions at and above the position of the section
            // to be added, one at a time starting from the end and going
            // down until current section
            for (int i = Sections.Count - 1; i >= posNewSection; i--)
            {
                Section sectionToMove = GetSectionByPosition(i);
                sectionToMove.SetPosition(sectionToMove.GetPosition() + 1);
            }

            // Add the new section to the list
            Sections.Add(section);

            // Now set the startRows for each section, starting at the current
            // position and moving upward
            SetStartRowsByPosition(posNewSection);

            UpdateDate();
        }

        public void RemoveSection(Section section)
        {
            if (Sections.Contains(section))
            {
                int posOldSection = section.GetPosition();

                // Decrement positions above the position of the section
                // to be removed, one at a time starting from the above the
                // removed section and going up until end
                for (int i = posOldSection + 1; i <= Sections.Count - 1; i++)
                {
                    Section sectionToMove = GetSectionByPosition(i);
                    sectionToMove.SetPosition(sectionToMove.GetPosition() - 1);
                }

                // Remove the section
                Sections.Remove(section);

                // Update the startRows for each section
                SetStartRowsByPosition(posOldSection);

                UpdateDate();
            }
            else
            {
                throw new ArgumentOutOfRangeException("section", "The passed section does not exist in this module.");
            }
        }


        private Section GetSectionByPosition(int position)
        {
            foreach (Section section in Sections)
            {
                if (section.GetPosition() == position)
                {
                    return section;
                }
            }
            return null;
        }

        private void SetStartRowsByPosition(int startPosition)
        {
            for (int i = startPosition; i <= Sections.Count - 1; i++)
            {
                Section sectionToUpdateStartRow = GetSectionByPosition(i);
                if (i == 0)
                {
                    // if this is the first section, then startRow will always be 1
                    sectionToUpdateStartRow.SetStartRow(Constants.defaultFirstRow);
                }
                else
                {
                    // otherwise, startRow = startRow(previous) + numRows(previous)
                    Section previousSection = GetSectionByPosition(i - 1);
                    int startRow = previousSection.GetStartRow() +
                        previousSection.GetRowCounter();
                    sectionToUpdateStartRow.SetStartRow(startRow);
                }
            }
        }

        // TODO: IMPORTANT - since we have a dateLastModified field, we'll need to invoke methods that would
        // update fields and properties, enabling us to update this field. Or else, by updating properties
        // directly, this field won't get updated.

        private void UpdateDate()
        {
            DateLastModified = DateTime.Now;
        }

        #endregion
    }
}
