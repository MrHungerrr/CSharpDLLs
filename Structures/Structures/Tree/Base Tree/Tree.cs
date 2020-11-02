using System;
using System.Collections;
using System.Collections.Generic;

namespace Vkimow.Structures.Trees.Base
{
    public class Tree<T> : IEnumerable<TreeElement<T>>
    { 
        public TreeElement<T> Root { get; }
        public bool IsEmpty => _elements.Count == 0;
        public IReadOnlyCollection<TreeElement<T>> Elements => _elements;
        public TreeElement<T> LastElement { get; private set; }


        private HashSet<TreeElement<T>> _elements;


        public Tree()
        {
            Root = new TreeElement<T>();
            _elements = new HashSet<TreeElement<T>>();
        }


        #region Logic

        #region Add

        private void Add(T content)
        {
            TreeElement<T> new_element = new TreeElement<T>(content);

            _elements.Add(new_element);

            LastElement = new_element;
        }

        public void Add(T content, TreeElement<T> parent)
        {
            if (_elements.Contains(parent) || parent == Root)
            {
                Add(content);
                parent.AddChild(LastElement);
            }
            else
            {
                throw new Exception("Нет такого Родителя");
            }
        }

        public void Add(Tree<T> treeToAdd, TreeElement<T> parent)
        {
            if (treeToAdd == null)
                throw new NullReferenceException();

            if (treeToAdd == this)
                throw new ArgumentException();

            if (_elements.Contains(parent) || parent == Root)
            {
                parent.TakeChildrenOf(treeToAdd.Root);
                _elements.UnionWith(treeToAdd._elements);
            }
            else
            {
                throw new Exception("Нет такого Родителя");
            }

        }

        public void AddToRoot(T content)
        {
            Add(content, Root);
        }

        public void AddToRoot(Tree<T> treeToAdd)
        {
            Add(treeToAdd, Root);
        }

        #endregion

        #region Remove
        /// <summary>
        /// Удаляет элемент. Новым родителем его детей становится родитель удаленного элемента.
        /// </summary>
        public void RemoveElementSafely(TreeElement<T> element)
        {
            if (_elements.Contains(element))
            {
                element.Parent.TakeChildrenOf(element);
                element.Parent.RemoveChild(element);
                _elements.Remove(element);
            }
            else
            {
                throw new Exception("Нет такого элемента");
            }
        }

        /// <summary>
        /// Удаляет ветку начиная с данного элемента.
        /// </summary>
        public void RemoveBrenchAfter(TreeElement<T> startElement)
        {
            if (_elements.Contains(startElement))
            {
                var elementsToDelete = startElement.GetElementsDown();
                elementsToDelete.Add(startElement);
                _elements.ExceptWith(elementsToDelete);

                startElement.RemoveParent();
            }
            else
            {
                throw new Exception("Нет такого элемента");
            }
        }

        /// <summary>
        /// Удаляет ветку, которой принадлежит "лист".
        /// </summary>
        public void RemoveBrenchWithLeaf(TreeElement<T> leaf)
        {
            if (leaf.HasChildren)
                throw new Exception("Элемент не является крайним");

            if (!_elements.Contains(leaf))
                throw new Exception("Нет такого элемента");


            var parent = leaf.Parent;
            var elementToDelete = leaf;

            while (parent.Children.Count == 1 && parent != Root)
            {
                elementToDelete = parent;
                parent = parent.Parent;
            }

            RemoveBrenchAfter(elementToDelete);
        }
        #endregion


        #region Get
        public List<TreeElement<T>> GetLeafs()
        {
            var leafs = new List<TreeElement<T>>();

            foreach (var element in this)
            {
                if (!element.HasChildren)
                    leafs.Add(element);
            }

            return leafs;
        }
        #endregion
        #endregion


        public override string ToString()
        {
            string res = $"Tree {{{_elements.Count}}}";
            int index = 0;

            foreach(var element in this)
            {
                res += $"\n{index++}) {element}\tParent {element.Parent}";
            }

            return res;
        }


        #region Enumerable

        public IEnumerator<TreeElement<T>> GetEnumerator()
        {
            foreach(TreeElement<T> element in this.BranchIterator())
                yield return element;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            foreach (TreeElement<T> element in this.BranchIterator())
                yield return element;
        }
        #endregion
    }
}
