using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestionnaireSpecGenerator
{
    public class Tester
    {
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
                foreach (Section section in module.Sections)
                {
                    sections.Add(section);
                    Console.WriteLine("Section title:" + section.STitle);
                }
            }

            Console.WriteLine("----------");

            foreach (Section section in sections)
            {
                foreach (QuestionBlock question in section.QuestionBlocks)
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
            {
                QuestionBlock newQuestion = new QuestionBlock();
                newQuestion.Comments = "Q100_NEWNEWNEW!!!";
                newQuestion.ProgInst = "NEW NEW NEW NEW NEW NEW NEWDNEWNEWNEWNEWNENW";

                sections[0].QuestionBlocks.Add(newQuestion);
            }

            Console.WriteLine("----- Add new section to first module via module object (NOT list of sections) ---");

            if (modules.Count() > 0)
            {
                Section newSection = new Section();
                newSection.STitle = "AAAAAAAAAAAAAAAAAAAAAAA";
                newSection.QuestionBlocks = new List<QuestionBlock>();

                modules[0].Sections.Add(newSection);
            }

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
                foreach (Section section in module.Sections)
                {
                    sections.Add(section);
                    Console.WriteLine("Section title:" + section.STitle);
                }
            }

            Console.WriteLine("----------");

            foreach (Section section in sections)
            {
                foreach (QuestionBlock question in section.QuestionBlocks)
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
