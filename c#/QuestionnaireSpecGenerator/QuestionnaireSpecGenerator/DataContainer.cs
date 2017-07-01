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
        public List<Module> modules;

        /// <summary>
        /// A list containing all the sections in the <see cref="Questionnaire"/> object.
        /// </summary>
        /// <value>
        /// The sections.
        /// </value>
        private List<Section> sections;

        /// <summary>
        /// A list containing all the questions in the <see cref="Questionnaire"/> object.
        /// </summary>
        /// <value>
        /// The question blocks.
        /// </value>
        private List<QuestionBlock> questionBlocks;

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
        /// The type dictionary. Alternative to switch on type for C#: https://stackoverflow.com/questions/708911/using-case-switch-and-gettype-to-determine-the-object
        /// </summary>
        private Dictionary<Type, QreObjTypes> typeDict = new Dictionary<Type, QreObjTypes>
        {
            {typeof(Module), QreObjTypes.Module},
            {typeof(Section), QreObjTypes.Section},
            {typeof(QuestionBlock), QreObjTypes.QuestionBlock},
            {typeof(Response), QreObjTypes.Response}
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="DataContainer"/> class. Empty constructor.
        /// </summary>
        public DataContainer()
        {

        }

        /// <summary>
        /// Initializes a new instance of the<see cref="DataContainer"/> class. 
        /// </summary>
        /// <param name="qre">The <see cref="Questionnaire"/> to store in the container. Required.</param>
        public DataContainer(Questionnaire qre)
        {
            InitializeQre(qre);
        }

        /// <summary>
        /// Initializes questionnaire element lists. Only works if qre had not already been initialized.
        /// </summary>
        /// <param name="qre"></param>
        public void InitializeQre(Questionnaire qre)
        {
            if (this.qre != null)
            {
                this.qre = qre;
                updated = new List<QreObjTypes>();
                moduleIds = new List<int>();
                sectionIds = new List<int>();
                questionBlockIds = new List<int>();
                modules = ExtractModules(qre);
                sections = ExtractSections(qre);
                questionBlocks = ExtractQuestionBlocks(qre);
            } 
            else
            {
                throw new ArgumentException("qre", "A qre has already been initialized. You need to create a new DataContainer to hold the new qre object.");
            }
        }

        /// <summary>
        /// Updates <see cref="updated"/> with the object types that have been recently updated. If the passed <see cref="type"/>
        /// already exists, this does nothing.
        /// </summary>
        /// <param name="type">The type.</param>
        private void FlagUpdate(QreObjTypes type)
        {
            // defensive code
            if (updated == null)
            {
                updated = new List<QreObjTypes>();
            }

            if (!updated.Contains(type))
            {
                updated.Add(type);
            }
        }

        // TODO: Try to remove the redundancies in the 3 code adds below

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
            List<Module> modules = new List<Module>();
            if (qre.Children != null)
            {
                FlagUpdate(QreObjTypes.Module);
                modules = qre.Children;
            }
            return modules;
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
            if (qre.Children != null && qre.Children.Count > 0)
            {
                foreach (Module module in qre.Children)
                {
                    if (module.Children != null && module.Children.Count > 0)
                    {
                        foreach (Section section in module.Children)
                        {
                            sections.Add(section);
                        }
                        FlagUpdate(QreObjTypes.Section);
                    }
                }
            }
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
            if (qre.Children != null && qre.Children.Count > 0)
            {
                foreach (Module module in qre.Children)
                {
                    if (module.Children != null && module.Children.Count > 0)
                    {
                        foreach (Section section in module.Children)
                        {
                            if (section.Children != null && section.Children.Count > 0)
                            {
                                foreach (QuestionBlock questionBlock in section.Children)
                                {
                                    questionBlocks.Add(questionBlock);
                                }
                                FlagUpdate(QreObjTypes.QuestionBlock);
                            }
                        }
                    }
                }
            }
            return questionBlocks;
        }

        /// <summary>
        /// Generic method that adds a new child object. If <see cref="QreObjBase.SelfId"/> of child is not unique, or if
        /// <see cref="QreObjBase.ParentId"/> does not exist, an exception is thrown. 
        /// <para>
        /// Compatible types must extend the <see cref="QreObjBase"/> class (i.e. must be <see cref="Module"/>, 
        /// <see cref="Section"/>, or <see cref="QuestionBlock"/>).
        /// </para>
        /// <para>Note: To be called via the Object Generator UI.</para>
        /// </summary>
        /// <typeparam name="T">Must extend <see cref="QreObjBase"/></typeparam>
        /// <param name="child">The child object to add.</param>
        /// <param name="position">The position.</param>
        /// <exception cref="ArgumentOutOfRangeException">obj - The object passed contains a duplicate self id</exception>
        /// <exception cref="ArgumentOutOfRangeException">obj - The object passed contains a duplicate self id</exception>
        public void AddChild<T>(T child, int position) where T : QreObjBase
        {
            try
            {
                // Generate self id and make sure it is unique and parent id exists
                QreObjTypes type = typeDict[child.GetType()];
                child.SelfId = GenerateRandomId(type);
                CheckIds(type, child.SelfId, child.ParentId); // DEBUGGING

                switch (type)
                {
                    case QreObjTypes.Module:
                        // Add module through parent
                        qre.AddChild((Module)(object)child, position);

                        // TEST: Check for referential equality
                        Console.WriteLine(ReferenceEquals(qre.Children[qre.Children.Count - 1], modules[modules.Count - 1])); // TRUE
                        break;
                    case QreObjTypes.Section:
                        // Add section through parent
                        Module mParent = GetModuleById(child.ParentId);
                        int mIndex = modules.IndexOf(mParent);
                        Section section = (Section)(object)child; // cast
                        modules[mIndex].AddChild(section, position);

                        // Update section list
                        sections.Add(section);

                        // TEST: Check for referential equality
                        int sIndex = modules[mIndex].Children.IndexOf(section);
                        Console.WriteLine(ReferenceEquals(modules[mIndex].Children[sIndex], sections[sections.Count - 1])); // TRUE
                        break;
                    case QreObjTypes.QuestionBlock:
                        // Add question through parent
                        Section sParent = GetSectionById(child.ParentId);
                        int spIndex = sections.IndexOf(sParent);
                        QuestionBlock question = (QuestionBlock)(object)child; // cast
                        sections[spIndex].AddChild(question, position);

                        // Update question list
                        questionBlocks.Add(question);
                        FlagUpdate(QreObjTypes.QuestionBlock);

                        // TEST: Check for referential equality
                        int qIndex = sections[spIndex].Children.IndexOf(question);
                        Console.WriteLine(ReferenceEquals(sections[spIndex].Children[qIndex], questionBlocks[questionBlocks.Count - 1])); // TRUE
                        break;
                    case QreObjTypes.Response:
                        // Add response through parent. Is not logged in its own Responses list
                        QuestionBlock qParent = GetQuestionById(child.ParentId);
                        int qbIndex = questionBlocks.IndexOf(qParent);
                        Response response = (Response)(object)child; // cast
                        questionBlocks[qbIndex].AddChild(response, position);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException("obj", "The object passed is not a compatible type. Object is type: " + child.GetType());
                }

                // Update object list
                FlagUpdate(type);
            }
            catch (ArgumentOutOfRangeException e)
            {
                Console.WriteLine("DataContainer.AddChild threw an exception: " + e + "\n Proceeding...");
            }
        }


        public void AddChildren<T>(List<Tuple<T, int>> children) where T : QreObjBase
        {
            foreach (Tuple<T, int> child in children)
            {
                AddChild(child.Item1, child.Item2);
            }
        }

        ///// <summary>
        ///// Adds a new module. Called via the <see cref="Module"/> constructor.
        ///// <para>Note: Called via the <see cref="Module"/> constructor.</para>
        ///// </summary>
        ///// <param name="module">The module to add.</param>
        ///// <exception cref="ArgumentOutOfRangeException">
        ///// section - The object passed contains a duplicate self id
        ///// </exception>
        ///// <exception cref="ArgumentOutOfRangeException">
        ///// section - The object passed contains an undefined parent id
        ///// </exception>
        //public void AddModule(Module module, int position)
        //{
        //    try
        //    {
        //        // Generate self id and make sure it is unique (for debugging purposes)
        //        module.SelfId = GenerateRandomId(QreObjTypes.Module);
        //        //CheckIds(QreObjTypes.Module, module.SelfId); // DEBUGGING

        //        // Add module through parent (this already updates the module list)
        //        qre.AddChild(module, position);
        //        FlagUpdate(QreObjTypes.Module);

        //        // TEST: Check for referential equality
        //        Console.WriteLine(ReferenceEquals(qre.Children[qre.Children.Count - 1], modules[modules.Count - 1])); // TRUE

        //    }
        //    catch (ArgumentOutOfRangeException e)
        //    {
        //        Console.WriteLine("DataContainer.AddModule threw an exception: " + e + "\n Proceeding...");
        //    }
        //}

        ///// <summary>
        ///// Adds a new section. If id of section is not unique, an exception is thrown.
        ///// <para>Note: Called via the <see cref="Section"/> constructor.</para>
        ///// </summary>
        ///// <param name="section">The section to add.</param>
        ///// <exception cref="ArgumentOutOfRangeException">
        ///// section - The object passed contains a duplicate self id
        ///// </exception>
        ///// <exception cref="ArgumentOutOfRangeException">
        ///// section - The object passed contains an undefined parent id
        ///// </exception>
        //public void AddSection(Section section, int position)
        //{
        //    try
        //    {
        //        // Generate self id and make sure it is unique and parent id exists
        //        section.SelfId = GenerateRandomId(QreObjTypes.Section);
        //        CheckIds(QreObjTypes.Section, section.SelfId, section.ParentId);

        //        // Add section through parent
        //        Module parent = GetModuleById(section.ParentId);
        //        int index = modules.IndexOf(parent);
        //        modules[index].AddChild(section, position);

        //        // Update section list
        //        sections.Add(section);
        //        FlagUpdate(QreObjTypes.Section);

        //        // TEST: Check for referential equality
        //        int sIndex = modules[index].Children.IndexOf(section);
        //        Console.WriteLine(ReferenceEquals(modules[index].Children[sIndex], sections[sections.Count - 1])); // TRUE
        //    }
        //    catch (ArgumentOutOfRangeException e)
        //    {
        //        Console.WriteLine("DataContainer.AddSection threw an exception: " + e + "\n Proceeding...");
        //    }
        //}

        ///// <summary>
        ///// Adds the new question. If id of question is not unique, an exception is thrown.
        ///// <para>Note: Called via the <see cref="QuestionBlock"/> constructor.</para>
        ///// </summary>
        ///// <param name="question">The question to add.</param>
        ///// <exception cref="ArgumentOutOfRangeException">
        ///// section - The object passed contains a duplicate self id
        ///// </exception>
        ///// <exception cref="ArgumentOutOfRangeException">
        ///// section - The object passed contains an undefined parent id
        ///// </exception>
        //public void AddQuestion(QuestionBlock question, int position)
        //{
        //    try
        //    {
        //        // Generate self id and make sure it is unique and parent id exists
        //        question.SelfId = GenerateRandomId(QreObjTypes.QuestionBlock);
        //        CheckIds(QreObjTypes.QuestionBlock, question.SelfId, question.ParentId);

        //        // Add question through parent
        //        Section parent = GetSectionById(question.SelfId);
        //        int index = sections.IndexOf(parent);
        //        sections[index].AddChild(question, position);

        //        // Update question list
        //        questionBlocks.Add(question);
        //        FlagUpdate(QreObjTypes.QuestionBlock);

        //        // TEST: Check for referential equality
        //        int qIndex = sections[index].Children.IndexOf(question);
        //        Console.WriteLine(ReferenceEquals(sections[index].Children[qIndex], questionBlocks[questionBlocks.Count - 1])); // TRUE
        //    }
        //    catch (ArgumentOutOfRangeException e)
        //    {
        //        Console.WriteLine("DataContainer.AddQuestion threw an exception: " + e + "\n Proceeding...");
        //    }
        //}

        /// <summary>
        /// Checks whether the self id is unique, and whether the parent id already exists. Raises an exception if either
        /// self id already exists, or if parent id does not.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="id">The self identifier.</param>
        /// <param name="pId">The parent identifier.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// id - The object passed contains a duplicate self id
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// pId - The object passed contains an undefined parent id
        /// </exception>
        private void CheckIds(QreObjTypes type, int id, int pId)
        {
            List<int> selfIds, parentIds;

            // Update self id list if necessary
            CheckUpdate(type);

            // Assign self id list, update parent id list if necessary, and assign parent id list
            switch (type)
            {
                case QreObjTypes.Module: // no parent here
                    selfIds = moduleIds;
                    parentIds = null;
                    break;
                case QreObjTypes.Section:
                    selfIds = sectionIds;
                    CheckUpdate(QreObjTypes.Module);
                    parentIds = moduleIds;
                    break;
                case QreObjTypes.QuestionBlock:
                    selfIds = questionBlockIds;
                    CheckUpdate(QreObjTypes.Section);
                    parentIds = sectionIds;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("type", "Incompatible type passed. " +
                        "Only accepts Module, Section, and Questionblock qre object types.");
            }

            // Confirm object id is unique (i.e. does not already exist)
            if (selfIds.Contains(id))
            {
                throw new ArgumentOutOfRangeException("section",
                    "The object " + type + " passed contains a duplicate id: " + id);
            }

            // Confirm parent id exists (for non-Module objects)
            if (parentIds != null && !parentIds.Contains(pId))
            {
                throw new ArgumentOutOfRangeException("section",
                    "The object " + type + " passed contains an undefined parent id: " + pId);
            }
        }

        /// <summary>
        /// Checks if the id lists are up to date, and if not, updates them.
        /// </summary>
        /// <param name="type">The qre object type to check.</param>
        /// <returns>
        /// Returns <c>true</c> if any id list has been updated. Otherwise,
        /// returns <c>false</c>.
        /// </returns>
        private bool CheckUpdate(QreObjTypes type)
        {
            List<int> ids = new List<int>();

            switch (type)
            {
                case QreObjTypes.Module:
                    if (updated.Contains(type))
                    {
                        foreach (Module module in modules)
                        {
                            ids.Add(module.SelfId);
                        }
                        moduleIds = ids;
                        updated.Remove(type);
                        return true;
                    }
                    return false;
                case QreObjTypes.Section:
                    if (updated.Contains(type))
                    {
                        foreach (Section section in sections)
                        {
                            ids.Add(section.SelfId);
                        }
                        sectionIds = ids;
                        updated.Remove(type);
                        return true;
                    }
                    return false;
                case QreObjTypes.QuestionBlock:
                    if (updated.Contains(type))
                    {
                        foreach (QuestionBlock question in questionBlocks)
                        {
                            ids.Add(question.SelfId);
                        }
                        questionBlockIds = ids;
                        updated.Remove(type);
                        return true;
                    }
                    return false;
                default:
                    throw new ArgumentOutOfRangeException("type", "Incompatible type passed. " +
                        "Only accepts Module, Section, and Questionblock qre object types.");
            }
        }

        /// <summary>
        /// Generates the random identifier code for a given questionnaire item type. Looks through an internalized
        /// list of ids currently taken for a provided type, and outputs an unused id for that type.
        /// <para>
        /// <b>Definition:</b> A non-zero, positive <see cref="int"/> with an upper limit of <see cref="int.MaxValue"/>.
        /// </para>
        /// </summary>
        /// <param name="type">
        /// The type of the object requesting a unique identifier. Must be one of:
        /// <list type="number">
        /// <item><description><see cref="QreObjTypes.Module"/></description></item>
        /// <item><description><see cref="QreObjTypes.Section"/></description></item>
        /// <item><description><see cref="QreObjTypes.QuestionBlock"/></description></item>
        /// </list>
        /// </param>
        /// <returns>A unique id for the given <c>Type</c>. Returns <c>-1</c> if an exception occurs.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// type - Incompatible type passed. Only accepts Module, Section, and Questionblock qre object types.
        /// </exception>
        private int GenerateRandomId(QreObjTypes type)
        {
            List<int> usedIds;
            try
            {
                usedIds = GetIds(type);
                int i = 1;
                while (true)
                {
                    if (usedIds.BinarySearch(i) < 0)
                    {
                        return i;
                    }
                    else
                    {
                        i++;
                    }
                }

            }
            catch (ArgumentOutOfRangeException e)
            {
                Console.WriteLine("GenerateRandomId threw an exception: " + e + "\n Returning -1...");
                return -1;
            }
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
        public Module GetModuleById(int id)
        {
            foreach (Module module in modules)
            {
                if (module.SelfId == id)
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
        public Section GetSectionById(int id)
        {
            foreach (Section section in sections)
            {
                if (section.SelfId == id)
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
        public QuestionBlock GetQuestionById(int id)
        {
            foreach (QuestionBlock question in questionBlocks)
            {
                if (question.SelfId == id)
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
            switch (type)
            {
                case QreObjTypes.Module:
                    CheckUpdate(type);
                    return moduleIds;
                case QreObjTypes.Section:
                    CheckUpdate(type);
                    return sectionIds;
                case QreObjTypes.QuestionBlock:
                    CheckUpdate(type);
                    return questionBlockIds;
                default:
                    throw new ArgumentOutOfRangeException("type", "Incompatible type passed. " +
                        "Only accepts Module, Section, and Questionblock qre object types.");
            }
        }

    }
}
