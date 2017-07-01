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
    public class Questionnaire : QuestionnaireObject<Module>
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

        // Storage of sets. This is above the specification of the questionnaire. Storage of
        // user's customized sets re-usable in other questionnaires
        public List<SectionSet> SSets { get; set; }
        public List<QuestionSet> QSets { get; set; }
        public List<ResponseSet> RSets { get; set; }

        private void InitializeLists()
        {
            SSets = new List<SectionSet>();
            QSets = new List<QuestionSet>();
            RSets = new List<ResponseSet>();
            Children = new List<Module>();
        }

        /// <summary>
        /// Prevents a default instance of the <see cref="Questionnaire"/> class from being created. Used for deserialization by
        /// <see cref="JsonHandler"/>.
        /// </summary>
        private Questionnaire()
        {
            InitializeLists();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Questionnaire"/> class.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <param name="desc">The desc.</param>
        /// <param name="client">The client.</param>
        /// <param name="team">The team.</param>
        /// <param name="owningOffice">The owning office.</param>
        /// <param name="country">The country.</param>
        /// <param name="modules">The list of modules to add, with a position for each. If a position is not specified,
        /// module will be added to end by default.</param>
        public Questionnaire(string title, string desc, string client, List<string> team,
            Office owningOffice, Country country, List<Tuple<Module, int>> modules) // use a tuple object to store position (won't be saved in child object anyway)
        {
            DateCreated = DateTime.Now;

            Title = title;
            Desc = desc;
            Client = client;
            Team = team;
            OwningOffice = owningOffice;
            Country = country;

            InitializeLists();

            // For a questionnaire object, these are irrelevant. Set to 0.
            SelfId = 0;
            ParentId = 0;

            // Add the modules
            if (modules != null)
            {
                AddChildren(modules);
            }
        }

        #region methods

        public override void UpdateDate()
        {
            DateModified = DateTime.Now;
        }

        #endregion
    }
}
