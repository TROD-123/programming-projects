using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestionnaireSpecGenerator
{
    /// <summary>
    /// Provides the methods for a parent list of child objects.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="QuestionnaireSpecGenerator.QreObjBase" />
    public class QuestionnaireObject<T> : QreObjBase
        where T : QreObjBase
    {
        /// <summary>
        /// List of children belonging to the parent class. Sorted by position, determined during add.
        /// </summary>
        public List<T> Children { get; set; }

        /// <summary>
        /// Adds the child object to the list of children for the parent class, at the position specified (default set to
        /// end). Sets the child's <see cref="QreObjBase.ParentId" /> to the parent's <see cref="QreObjBase.SelfId" />.
        /// <para>
        /// Note, when creating the child to be added, no need to put down a Parent ID, since that is
        /// done here!
        /// </para>
        /// </summary>
        /// <param name="child">The child object to add. Child class must implement <see cref="QreObjBase" /></param>
        /// <param name="position">The position in which to insert the child into the parent's <see cref="Children"/> list.</param>
        public void AddChild(T child, int position = -1)
        {
            child.ParentId = SelfId;
            if (position != -1 && position <= Children.Count)
            {
                Children.Insert(position, child);
            }
            else if (position == -1 || position > Children.Count)
            {
                Children.Add(child);
            }
            else
            {
                throw new ArgumentOutOfRangeException("position", "The passed position is invalid. Must be 0 or greater.");
            }
            UpdateDate();
        }

        /// <summary>
        /// Adds a list of child objects to the list of children for the parent class, at the position specified.
        /// </summary>
        /// <param name="children">The list of children to add, represented as a <see cref="Tuple"/> with a child of 
        /// type <see cref="QuestionnaireObject{T}"/> and an <see cref="int"/> for the position.</param>
        /// <exception cref="ArgumentOutOfRangeException">children - The passed list is null. No action taken.</exception>
        public void AddChildren(List<Tuple<T,int>> children)
        {
            if (children != null)
            {
                foreach (Tuple<T, int> child in children)
                {
                    T obj = child.Item1;
                    int index = child.Item2;
                    AddChild(obj, index);

                }
                UpdateDate();
            } else
            {
                throw new ArgumentOutOfRangeException("children", "The passed list is null. No action taken.");
            }
        }

        /// <summary>
        /// Removes the child object from the parent list.
        /// </summary>
        /// <param name="child">The child to remove. Child class must implement <see cref="QreObjBase"/></param>
        /// <exception cref="ArgumentOutOfRangeException">child - The passed child does not exist in this parent.</exception>
        public void RemoveChild(T child)
        {
            if (Children.Remove(child))
            {
                UpdateDate();
            }
            else
            {
                throw new ArgumentOutOfRangeException("child", "The passed child does not exist in this parent.");
            }
        }

        // Still need to implement
        public void ChangeParent(int newParentId)
        {
            ParentId = newParentId;
        }

        public override void UpdateDate()
        {
            DateModified = DateTime.Now;
        }
    }
}
