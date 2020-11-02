using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Vkimow.Structures.Trees.Decision
{
    public class TreeElement<T>
    {
        public T Content { get; private set; }
        public int PathCount => _pathCount;
        public IReadOnlyList<TreeElement<T>> Parents => _parents;
        public IReadOnlyList<TreeElement<T>> Children => _children;
        public bool HasParents => (_parents?.Count + 0) > 0;
        public bool HasChildren => _children.Count > 0;

        private List<TreeElement<T>> _parents;
        private List<TreeElement<T>> _children;
        private int _pathCount;

        internal TreeElement(T content)
        {
            Content = content;
            _parents = new List<TreeElement<T>>();
            _children = new List<TreeElement<T>>();
        }

        internal TreeElement()
        {
            Content = default(T);
            _parents = null;
            _children = new List<TreeElement<T>>();
            _pathCount = 1;
        }

        #region Add

        #region Child
        internal void AddChild(TreeElement<T> child)
        {
            if (child == null)
                throw new NullReferenceException("Сука, Ребенок хуевый!");

            if (_parents?.Contains(child) ?? false)
                throw new Exception("Добавляем ребенка, который является на самом деле нашим родителем!");

            _children.Add(child);
            child._parents.Add(this);

            child.PathCountRecalulate();
        }

        internal void AddChildren(IEnumerable<TreeElement<T>> children)
        {
            try
            {
                if (children == null)
                    throw new NullReferenceException();

                foreach (TreeElement<T> child in children)
                {
                    AddChild(child);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        #endregion

        #endregion

        #region Remove
        internal void RemoveChild(TreeElement<T> child)
        {
            _children.Remove(child);
            child._parents.Remove(this);
            child.PathCountRecalulate();
        }

        internal void RemoveAllChildren()
        {
            foreach (TreeElement<T> child in _children)
            {
                child._parents.Remove(this);
                child.PathCountRecalulate();
            }

            _children = new List<TreeElement<T>>();
        }


        internal void RemoveParent(TreeElement<T> parent)
        {
            _parents.Remove(parent);
            parent._children.Remove(this);
            PathCountRecalulate();
        }

        internal void RemoveAllParents()
        {
            foreach (TreeElement<T> parent in _parents)
            {
                parent._children.Remove(this);
            }

            PathCountRecalulate();
            _parents = new List<TreeElement<T>>();
        }

        #endregion


        private void PathCountRecalulate()
        {
            _pathCount = 0;

            if (HasParents)
            {
                foreach (var parent in _parents)
                {
                    _pathCount += parent._pathCount;
                }
            }

            if (HasChildren)
            {
                foreach (var child in _children)
                {
                    child.PathCountRecalulate();
                }
            }
        }


        public List<TreeElement<T>> GetElementsDown()
        {
            var elements = new List<TreeElement<T>>();

            foreach (var element in TreeIterator.GetElementsDown(this))
                elements.Add(element);

            return elements;
        }

        public void SetContent(T content)
        {
            Content = content;
        }


        public override string ToString()
        {
            if(_parents != null)
                return Content.ToString();

            return "Root";
        }
    }
}
