using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestionnaireSpecGenerator
{
    /// <summary>
    /// A container for storing and persisting questionnaire data during a user's session. Any modifications
    /// the user makes to the qre get stored in the questionnaire object within the container.
    /// <para>
    /// When the user saves their questionnaire configuration, the questionnaire object in this container
    /// gets serialized. Loaded, or deserialized, questionnaires also get stored here.
    /// </para>
    /// </summary>
    public class DataContainer
    {
        /// <summary>
        /// The global questionnaire object which contains all questionnaire data, including the modules,
        /// sections, questions, and responses. This object gets sent for serialization when it is saved.
        /// </summary>
        /// <value>
        /// The qre.
        /// </value>
        private Questionnaire qre;

        /// <summary>
        /// A list containing all the modules in the <see cref="Questionnaire"/> object.
        /// </summary>
        /// <value>
        /// The modules.
        /// </value>
        private static List<Module> modules;

        /// <summary>
        /// A list containing all the sections in the <see cref="Questionnaire"/> object.
        /// </summary>
        /// <value>
        /// The sections.
        /// </value>
        private static List<Section> sections;

        /// <summary>
        /// A list containing all the questions in the <see cref="Questionnaire"/> object.
        /// </summary>
        /// <value>
        /// The question blocks.
        /// </value>
        private static List<QuestionBlock> questionBlocks;

        /// <summary>
        /// Flags denoting whether the questionnaire objects have been recently updated. Used for id indexing.
        /// </summary>
        private List<QreObjTypes> updated;

        /// <summary>
        /// The list of module ids. Up to date unless <see cref="QreObjTypes.Module"/> is flagged in <see cref="updated"/>.
        /// </summary>
        private List<int> moduleIds;

        /// <summary>
        /// The list of section ids. Up to date unless <see cref="QreObjTypes.Section"/> is flagged in <see cref="updated"/>.
        /// </summary>
        private List<int> sectionIds;

        /// <summary>
        /// The list of question ids. Up to date unless <see cref="QreObjTypes.QuestionBlock"/> is flagged in <see cref="updated"/>.
        /// </summary>
        private List<int> questionBlockIds;


        /// <summary>
        /// Initializes a new instance of the <see cref="DataContainer"/> class. Also initializes questionnaire
        /// element lists.
        /// </summary>
        /// <param name="qre">The <see cref="Questionnaire"/> to store in the container. Required.</param>
        public DataContainer(Questionnaire qre)
        {
            this.qre = qre;
            updated = new List<QreObjTypes>();
            modules = ExtractModules(qre);
            sections = ExtractSections(qre);
            questionBlocks = ExtractQuestionBlocks(qre);
        }

        /// <summary>
        /// Updates <see cref="updated"/> with the object types that have been recently updated. If the passed <see cref="type"/>
        /// already exists, this does nothing.
        /// </summary>
        /// <param name="type">The type.</param>
        private void FlagUpdate(QreObjTypes type)
        {
            if (updated == null)
            {
                updated = new List<QreObjTypes>();
            }

            if (!updated.Contains(type))
            {
                updated.Add(type);
            }
        }

        /// <summary>
        /// Extracts all of the modules in the qre. Only used as a helper during initialization.
        /// </summary>
        /// <param name="qre">The questionnaire object.</param>
        /// <returns>
        /// The list of all modules contained in the questionnaire. If there are
        /// no modules, then returns an empty list.
        /// </returns>
        private List<Module> ExtractModules(Questionnaire qre)
        {
            FlagUpdate(QreObjTypes.Module);
            return qre.Modules;
        }

        /// <summary>
        /// Extracts all of the sections in the questionnaire. Only used as a helper during initialization.
        /// </summary>
        /// <param name="qre">The questionnaire object.</param>
        /// <returns>
        /// The list of all sections contained in the questionnaire, across all modules. If there are
        /// no sections, then returns an empty list.
        /// </returns>
        private List<Section> ExtractSections(Questionnaire qre)
        {
            List<Section> sections = new List<Section>();
            foreach (Module module in qre.Modules)
            {
                foreach (Section section in module.Sections)
                {
                    sections.Add(section);
                }
            }
            FlagUpdate(QreObjTypes.Section);
            return sections;
        }

        /// <summary>
        /// Extracts all of the questions in the questionnaire. Only used as a helper during initialization.
        /// </summary>
        /// <param name="qre">The questionnaire object.</param>
        /// <returns>
        /// The list of all questions contained in the questionnaire, across all sections and modules. If there are
        /// no questions, then returns an empty list.
        /// </returns>
        private List<QuestionBlock> ExtractQuestionBlocks(Questionnaire qre)
        {
            List<QuestionBlock> questionBlocks = new List<QuestionBlock>();
            foreach (Module module in qre.Modules)
            {
                foreach (Section section in module.Sections)
                {
                    foreach (QuestionBlock questionBlock in section.QuestionBlocks)
                    {
                        questionBlocks.Add(questionBlock);
                    }
                }
            }
            FlagUpdate(QreObjTypes.QuestionBlock);
            return questionBlocks;
        }


        // TODO: DataContainer should also provide means of accessing and manipulating qre elements. Set
        // up getters and setters for all objects here. Make them accessible via id.

        /// <summary>
        /// Gets the module by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>
        /// The matching <see cref="Module"/> with the provided id, or <c>null</c> if 
        /// the module of the given id does not exist.
        /// </returns>
        public static Module GetModuleById(int id)
        {
            foreach (Module module in modules)
            {
                if (module.MId == id)
                {
                    return module;
                }
            }
            return null;
        }

        /// <summary>
        /// Gets the section by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>
        /// The matching <see cref="Section"/> with the provided id, or <c>null</c> if 
        /// the section of the given id does not exist.
        /// </returns>
        public static Section GetSectionById(int id)
        {
            foreach (Section section in sections)
            {
                if (section.SId == id)
                {
                    return section;
                }
            }
            return null;
        }

        /// <summary>
        /// Gets the question  by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>
        /// The matching <see cref="QuestionBlock"/> with the provided id, or <c>null</c> if 
        /// the question of the given id does not exist.
        /// </returns>
        public static QuestionBlock GetQuestionById(int id)
        {
            foreach (QuestionBlock question in questionBlocks)
            {
                if (question.QId == id)
                {
                    return question;
                }
            }
            return null;
        }


        // The below are for the random id generators (used in the Toolbox class)

        /// <summary>
        /// Gets the complete list of ids of the passed type. If the specified list has recently been updated, this
        /// indexes and returns the refreshed id list. Otherwise, this returns the list directly without refreshing.
        /// </summary>
        /// <param name="type">
        /// The questionnaire object type. Must be one of:
        /// <list type="bullet">
        /// <item><description><see cref="QreObjTypes.Module"/></description></item>
        /// <item><description><see cref="QreObjTypes.Section"/></description></item>
        /// <item><description><see cref="QreObjTypes.QuestionBlock"/></description></item>
        /// </list>
        /// </param>
        /// <returns>List of ids for the passed type</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// type - Incompatible type passed. Only accepts Module, Section, and Questionblock qre object types.
        /// </exception>
        internal List<int> GetIds(QreObjTypes type)
        {
            List<int> ids = new List<int>();
            switch (type)
            {
                case QreObjTypes.Module:
                    if (updated.Contains(type))
                    {
                        foreach (Module module in modules)
                        {
                            ids.Add(module.MId);
                        }
                        updated.Remove(type);
                        return moduleIds = ids;
                    }
                    else
                    {
                        return moduleIds;
                    }
                case QreObjTypes.Section:
                    if (updated.Contains(type))
                    {
                        foreach (Section section in sections)
                        {
                            ids.Add(section.SId);
                        }
                        updated.Remove(type);
                        return sectionIds = ids;
                    }
                    else
                    {
                        return sectionIds;
                    }
                case QreObjTypes.QuestionBlock:
                    if (updated.Contains(type))
                    {
                        foreach (QuestionBlock question in questionBlocks)
                        {
                            ids.Add(question.QId);
                        }
                        updated.Remove(type);
                        return questionBlockIds = ids;
                    }
                    else
                    {
                        return questionBlockIds;
                    }
                default:
                    throw new ArgumentOutOfRangeException("type", "Incompatible type passed. " +
                        "Only accepts Module, Section, and Questionblock qre object types.");
            }
        }
    }
}
