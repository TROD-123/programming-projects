using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestionnaireSpecGenerator
{
    /// <summary>
    /// Base class for a questionnaire object.
    /// </summary>
    public abstract class QreObjBase
    {
        /// <summary>
        /// Object self identifier.
        /// <para>Requirements:</para>
        /// <list type="number">
        ///     <item>
        ///         <description>Must not be <c>null</c>.</description>
        ///     </item>
        ///     <item>
        ///         <description>Must be a unique <c>int</c> per object, across all the same object type.</description>
        ///     </item>
        /// </list>
        /// </summary>
        public int SelfId { get; set; }

        /// <summary>
        /// Object's parent identifier.
        /// </summary>
        public int ParentId { get; set; }

        /// <summary>
        /// Object creation date and time.
        /// </summary>
        public DateTime DateCreated { get; set; }

        /// <summary>
        /// Date and time object was last modified.
        /// </summary>
        public DateTime DateModified { get; set; }

        /// <summary>
        /// Updates the date and time the object was last modified.
        /// </summary>
        public abstract void UpdateDate();
    }
}
