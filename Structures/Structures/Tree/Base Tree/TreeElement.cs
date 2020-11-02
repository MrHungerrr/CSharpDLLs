using System;
using System.Collections;
using System.Collections.Generic;

namespace Vkimow.Structures.Trees.Base
{
    public class TreeElement<T> : IEnumerable<TreeElement<T>>
    {
        public T Content { get; private set; }
        public TreeElement<T> Parent { get; private set; }
        public IReadOnlyList<TreeElement<T>> Children => _children;
        public bool HasChildren => _children.Count > 0;


        private List<TreeElement<T>> _children;


        internal TreeElement(T content, TreeElement<T> parent)
        {
            Content = content;
            Parent = parent;
            _children = new List<TreeElement<T>>();
        }

        internal TreeElement(T content)
        {
            Content = content;
            _children = new List<TreeElement<T>>();
        }

        internal TreeElement()
        {
            Content = default(T);
            _children = new List<TreeElement<T>>();
        }


        internal void AddChild(TreeElement<T> child)
        {
            if (child != null)
            {
                _children.Add(child);
                BecomeParentToChild(child);
            }
        }

        internal void AddChildren(IEnumerable<TreeElement<T>> children)
        {
            if (children != null)
            {
                _children.AddRange(children);

                foreach (TreeElement<T> child in children)
                {
                    BecomeParentToChild(child);
                }
            }
        }

        private void BecomeParentToChild(TreeElement<T> child)
        {
            if (child.Parent != null)
                throw new Exception("У элемента уже есть родитель");
            
            child.Parent = this;
        }

        internal void RemoveChild(TreeElement<T> child)
        {
            _children.Remove(child);
            child.Parent = null;
        }

        internal void RemoveAllChildren()
        {
            foreach (TreeElement<T> child in _children)
            {
                child.Parent = null;
            }

            _children = new List<TreeElement<T>>();
        }


        internal void TakeChildrenOf(TreeElement<T> previousParent)
        {
            _children.AddRange(previousParent._children);

            foreach (TreeElement<T> child in previousParent._children)
            {
                child.Parent = this;
            }
            
            previousParent._children = new List<TreeElement<T>>();
        }


        internal void RemoveParent()
        {
            Parent.RemoveChild(this);
        }

        internal List<TreeElement<T>> GetElementsDown()
        {
            var list = new List<TreeElement<T>>();

            foreach (var element in TreeIterator.GetElementsDown(this))
            {
                list.Add(element);
            }

            return list;
        }


        public void SetContent(T content)
        {
            Content = content;
        }



        public override string ToString()
        {
            if(Parent != null)
                return Content.ToString();

            return "Root";
        }

        #region IEnumerator
        public IEnumerator<TreeElement<T>> GetEnumerator()
        {
            return ((IEnumerable<TreeElement<T>>)_children).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }
}
