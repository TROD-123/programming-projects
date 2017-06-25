using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestionnaireSpecGenerator
{
    public class Tester
    {
        public static void TestGetObjectById(Questionnaire qre)
        {
            Console.WriteLine("-- Testing Get Object by Id --");
            DataContainer container = new DataContainer(qre);

            Module existsModule = container.GetModuleById(1);
            Console.WriteLine("Module id 1: " + existsModule.MTitle);

            Module nonExistsModule = container.GetModuleById(5);
            Console.WriteLine("Module id 5: Null? " + (nonExistsModule == null));

            Section existsSection = container.GetSectionById(1);
            Console.WriteLine("Section id 1: " + existsSection.STitle);

            Section nonExistsSection = container.GetSectionById(100);
            Console.WriteLine("Section id 100: Null? " + (nonExistsSection == null));

            QuestionBlock existsQuestion = container.GetQuestionById(2);
            Console.WriteLine("Question id 2: " + existsQuestion.QTitle);

            QuestionBlock nonExistsQuestion = container.GetQuestionById(6);
            Console.WriteLine("Question id 6: Null? " + (nonExistsQuestion == null));


            // here, a section is added to the container automatically after creation
            Console.WriteLine("-- Testing add a new section into first module, previously non-existant --");
            Section section = new Section(container, 1, 0)
            {
                STitle = "This is a newly added section!"
            };

            // here, a module is added to the container automatically after creation
            Console.WriteLine("-- Testing adding a new module automatically through constructor, previously non-existant --");
            Module module = new Module(container);

            Console.WriteLine("-- Testing adding a new question automatically through constructor, previously non-existant. To section id 3 --");
            QuestionBlock question = new QuestionBlock(container, 3);
            
        }

        public static void TestRandomIntGenerator(Questionnaire qre)
        {
            Console.WriteLine("-- Testing Generation of Random Ids --");
            DataContainer container = new DataContainer(qre);

            Console.WriteLine("-- Getting the current ids --");
            List<int> moduleIds = container.GetIds(QreObjTypes.Module);
            List<int> sectionIds = container.GetIds(QreObjTypes.Section);
            List<int> questionIds = container.GetIds(QreObjTypes.QuestionBlock);

            Console.Write("Module ids: ");
            foreach (int i in moduleIds)
            {
                Console.Write(i + " ");
            }

            Console.WriteLine();
            Console.Write("Section ids: ");
            foreach (int i in sectionIds)
            {
                Console.Write(i + " ");
            }

            Console.WriteLine();
            Console.Write("Question ids: ");
            foreach (int i in questionIds)
            {
                Console.Write(i + " ");
            }

            Console.WriteLine();
            Console.WriteLine("-- Generating random ids --");

            int moduleId = Toolbox.GenerateRandomId(container, QreObjTypes.Module);
            int sectionId = Toolbox.GenerateRandomId(container, QreObjTypes.Section);
            int questionId = Toolbox.GenerateRandomId(container, QreObjTypes.QuestionBlock);

            Console.WriteLine("New module id: " + moduleId);
            Console.WriteLine("New section id: " + sectionId);
            Console.WriteLine("New question id: " + questionId);

            Console.WriteLine();
            Console.WriteLine("-- End Testing Generation of Random Ids --");
           
        }


        /// <summary>
        /// Tests the modify qre existing objects indirectly. Purpose of this is to show that modifying the individual
        /// list of qre objects would be reflected in the parent qre object - as long as the lists are pointing to
        /// the qre reference that contains the lists (e.g. qre.Modules, or qre.Modules[0].Sections)
        /// </summary>
        /// <param name="qre">The qre.</param>
        public static void TestModifyQreExistingObjectsIndirectly(Questionnaire qre)
        {
            List<Module> modules = new List<Module>();
            List<Section> sections = new List<Section>();
            List<QuestionBlock> questions = new List<QuestionBlock>();


            foreach (Module module in qre.Modules)
            {
                modules.Add(module);
            }

            foreach (Module module in modules)
            {
                foreach (Section section in module.Children)
                {
                    sections.Add(section);
                    Console.WriteLine("Section title:" + section.STitle);
                }
            }

            Console.WriteLine("----------");

            foreach (Section section in sections)
            {
                foreach (QuestionBlock question in section.Children)
                {
                    questions.Add(question);
                    Console.WriteLine("Question comments: " + question.Comments);
                    Console.WriteLine("Question prog inst: " + question.ProgInst);
                }
            }

            Console.WriteLine("Initial counts: " + modules.Count() + " " + sections.Count() + " " + questions.Count());


            Console.WriteLine("---- CHANGE FIRST QUESTION COMMENTS AND PROG, AND FIRST SECTION TITLE------");

            if (questions.Count() > 0)
            {
                questions[0].Comments = "THE END IS NIGH!!!";
                questions[0].ProgInst = "fuck off";
            }

            if (sections.Count() > 0)
            {
                sections[0].STitle = "I've changed this property haha.";
            }

            Console.WriteLine("---- Checking lists ------");


            foreach (Section section in sections)
            {
                Console.WriteLine("Section title:" + section.STitle);
            }

            foreach (QuestionBlock question in questions)
            {
                Console.WriteLine("Question comments: " + question.Comments);
                Console.WriteLine("Question prog inst: " + question.ProgInst);
            }

            Console.WriteLine("---- Add new question to first section via section object (NOT list of questions) ---");

            if (sections.Count() > 0)
            //{
            //    QuestionBlock newQuestion = new QuestionBlock();
            //    newQuestion.Comments = "Q100_NEWNEWNEW!!!";
            //    newQuestion.ProgInst = "NEW NEW NEW NEW NEW NEW NEWDNEWNEWNEWNEWNENW";

            //    sections[0].QuestionBlocks.Add(newQuestion);
            //}

            Console.WriteLine("----- Add new section to first module via module object (NOT list of sections) ---");

            if (modules.Count() > 0)
            //{
            //    Section newSection = new Section(container);
            //    newSection.STitle = "AAAAAAAAAAAAAAAAAAAAAAA";
            //    newSection.QuestionBlocks = new List<QuestionBlock>();

            //    modules[0].Sections.Add(newSection);
            //}

            Console.WriteLine("--- Clear objects and Loop through qre elements again ----");

            modules.Clear();
            sections.Clear();
            questions.Clear();

            Console.WriteLine("Lengths: " + modules.Count() + " " + sections.Count() + " " + questions.Count());

            foreach (Module module in qre.Modules)
            {
                modules.Add(module);
            }

            foreach (Module module in modules)
            {
                foreach (Section section in module.Children)
                {
                    sections.Add(section);
                    Console.WriteLine("Section title:" + section.STitle);
                }
            }

            Console.WriteLine("----------");

            foreach (Section section in sections)
            {
                foreach (QuestionBlock question in section.Children)
                {
                    questions.Add(question);
                    Console.WriteLine("Question comments: " + question.Comments);
                    Console.WriteLine("Question prog inst: " + question.ProgInst);
                }
            }

            Console.WriteLine("Final counts: " + modules.Count() + " " + sections.Count() + " " + questions.Count());

        }


    }
}
