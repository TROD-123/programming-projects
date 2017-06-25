using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestionnaireSpecGenerator
{
    /// <summary>
    /// This class represents the highest level of the questionnaire object hierarchy.
    /// Contains general details about the questionnaire.
    /// </summary>
    public class Questionnaire
    {
        /// <summary>
        /// The title of the questionnaire
        /// </summary>
        public string Title { get; set; }


        /// <summary>
        /// The questionnaire description
        /// </summary>
        public string Desc { get; set; }

        /// <summary>
        /// The client
        /// </summary>
        public string Client { get; set; }

        /// <summary>
        /// The list of people involved in the project
        /// </summary>
        public List<string> Team { get; set; }

        /// <summary>
        /// The owning office
        /// </summary>
        public Office OwningOffice { get; set; }

        /// <summary>
        /// The country of the owning office
        /// </summary>
        public Country Country { get; set; }

        /// <summary>
        /// The module creation date and time
        /// </summary>
        public DateTime DateCreated { get; set; }

        /// <summary>
        /// The last time module was modified
        /// </summary>
        public DateTime DateLastModified { get; set; }

        // Storage of sets. This is above the specification of the questionnaire. Storage of
        // user's customized sets re-usable in other questionnaires
        public List<SectionSet> SSets { get; set; }
        public List<QuestionSet> QSets { get; set; }
        public List<ResponseSet> RSets { get; set; }

        // The start of the questionnaire object chain. The lower level objects in the questionnaire
        // spec can be accessed through here.
        public List<Module> Modules { get; set; }

        #region methods

        public void AddModule(Module module)
        {
            Modules.Add(module);
        }

        public void RemoveModule(Module module)
        {
            if (Modules.Contains(module))
            {
                Modules.Remove(module);
            }
            else
            {
                throw new ArgumentOutOfRangeException("module", "The passed module does not exist in this questionnaire.");
            }
        }

        #endregion
    }
}
