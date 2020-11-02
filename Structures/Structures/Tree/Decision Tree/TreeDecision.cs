using System;
using System.Collections;
using System.Collections.Generic;

namespace Vkimow.Structures.Trees.Decision
{
    public class TreeDecision<T> : IEnumerable<TreeElement<T>>
    {
        public TreeElement<T> Root { get; }
        public bool IsEmpty => _elements.Count == 0;
        public int BranchCount => _branchCount;
        public TreeElement<T> LastElement { get; private set; }


        private HashSet<TreeElement<T>> _elements;
        private int _branchCount;


        public TreeDecision()
        {
            Root = new TreeElement<T>();
            _elements = new HashSet<TreeElement<T>>();
            _branchCount = 1;
        }


        #region Logic

        #region Add

        private void Add(T content)
        {
            TreeElement<T> newElement = new TreeElement<T>(content);

            _elements.Add(newElement);

            LastElement = newElement;
        }

        public void Add(T content, TreeElement<T> parent)
        {
            if (_elements.Contains(parent) || parent == Root)
            {
                Add(content);

                if (parent.HasChildren)
                    _branchCount++;

                parent.AddChild(LastElement);
            }
            else
            {
                throw new Exception($"Нет такого родителя!\n {parent}");
            }
        }

        public void Add(TreeDecision<T> treeToAdd, TreeElement<T> parent)
        {
            if (treeToAdd == null)
                throw new NullReferenceException();

            if (treeToAdd == this)
                throw new ArgumentException();

            if (_elements.Contains(parent) || parent == Root)
            {
                _branchCount += parent.PathCount * treeToAdd._branchCount;

                if (!parent.HasChildren)
                    _branchCount -= parent.PathCount;

                parent.AddChildren(treeToAdd.Root.Children);
                treeToAdd.Root.RemoveAllChildren();
                _elements.UnionWith(treeToAdd._elements);
            }
            else
            {
                throw new Exception($"Нет такого родителя!\n {parent}");
            }
        }

        public void Add(T content, IEnumerable<TreeElement<T>> parents)
        {
            if (!_elements.IsSupersetOf(parents))
            {
                Add(content);

                foreach (var parent in parents)
                {
                    if (parent.HasChildren)
                        _branchCount++;

                    parent.AddChild(LastElement);
                }
            }
            else
            {
                throw new Exception($"Нет таких родителей!\n{parents.ToString()}");
            }
        }

        public void Add(TreeDecision<T> treeToAdd, IEnumerable<TreeElement<T>> parents)
        {
            if (treeToAdd == null)
                throw new NullReferenceException();

            if (treeToAdd == this)
                throw new ArgumentException();

            if (_elements.IsSupersetOf(parents))
            {
                foreach (var parent in parents)
                {
                    _branchCount += parent.PathCount * treeToAdd._branchCount;

                    if (!parent.HasChildren)
                        _branchCount -= parent.PathCount;

                    parent.AddChildren(treeToAdd.Root.Children);
                }

                treeToAdd.Root.RemoveAllChildren();
                _elements.UnionWith(treeToAdd._elements);
            }
            else
            {
                throw new Exception($"Нет таких родителей!\n{parents.ToString()}");
            }
        }

        public void AddToRoot(T content)
        {
            Add(content, Root);
        }

        public void AddToRoot(TreeDecision<T> treeToAdd)
        {
            Add(treeToAdd, Root);
        }

        public void AddToLeafs(T content)
        {
            if (!IsEmpty)
            {
                var leafs = GetLeafs();
                Add(content, leafs);
            }
            else
            {
                Add(content, Root);
            }
        }

        public void AddToLeafs(TreeDecision<T> treeToAdd)
        {
            if (!IsEmpty)
            {
                var leafs = GetLeafs();
                Add(treeToAdd, leafs);
            }
            else
            {
                Add(treeToAdd, Root);
            }
        }

        #endregion

        #region Remove
        /// <summary>
        /// Удаляет все ветки, к которым пренадлежит данный элемент
        /// </summary>
        /// <param name="element"></param>
        public void RemoveBranchesWith(TreeElement<T> element)
        {
            if (!_elements.Contains(element))
                throw new Exception("Нет такого элемента");


            foreach (var parent in element.Parents)
            {
                element.RemoveParent(parent);

                if (!parent.HasChildren)
                {
                    RemoveUp(parent);
                }
            }
            

            if (element.HasChildren)
            {
                foreach (var child in element.Children)
                {
                    element.RemoveChild(child);

                    if (!child.HasParents)
                    {
                        RemoveDown(child);
                    }
                }
            }

            _elements.Remove(element);
            RecalculateBrachCount();
        }



        /// <summary>
        /// Удаляет все ветки, которые расположены после данного элемента
        /// </summary>
        /// <param name="startElement"></param>
        public void RemoveBranchesAfter(TreeElement<T> startElement)
        {
            startElement.RemoveAllParents();
            RemoveDown(startElement);
            RecalculateBrachCount();
        }


        /// <summary>
        /// Удаляет ветку, которой пренадлежит "лист" - крайний элемент дерева
        /// </summary>
        /// <param name="leafElement"></param>
        public void RemoveBranchWithLeaf(TreeElement<T> leafElement)
        {
            if (leafElement.HasChildren)
                throw new Exception("Элемент не поcледний");

            RemoveUp(leafElement);

            RecalculateBrachCount();
        }


        private void RemoveDown(TreeElement<T> element)
        {
            if (!_elements.Contains(element))
                throw new Exception("Нет такого элемента");

            foreach (var child in element.Children)
            {
                element.RemoveChild(child);

                if (!child.HasParents)
                {
                    RemoveDown(child);
                }
            }

            _elements.Remove(element);
        }


        private void RemoveUp(TreeElement<T> element)
        {
            if (element == Root)
                return;

            if (_elements.Contains(element))
                throw new Exception("Нет такого элемента");


            foreach (var parent in element.Parents)
            {
                element.RemoveParent(parent);

                if (!parent.HasChildren)
                {
                    RemoveBranchWithLeaf(parent);
                }
            }

            _elements.Remove(element);
        }
        #endregion

        #region Get

        private List<TreeElement<T>> GetLeafs()
        {
            var leafs = new List<TreeElement<T>>();

            foreach (var element in this.BranchIteratorWithRoot())
            {
                if (!element.HasChildren)
                    leafs.Add(element);
            }

            return leafs;
        }

        #endregion

        #endregion


        private void RecalculateBrachCount()
        {
            _branchCount = 0;

            foreach(var element in this.BranchIteratorWithRoot())
            {
                if(!element.HasChildren)
                {
                    _branchCount += element.PathCount;
                }
            }
        }


        public override string ToString()
        {
            int spaceCount = 0;
            int elementsCount = _elements.Count;

            while (elementsCount > 0)
            {
                elementsCount /= 10;
                spaceCount++;
            }

            string res = $"Tree (Branches: {_branchCount}, Elements: {_elements.Count})";
            int index = 0;
            string format = $"D{spaceCount}";

            foreach (var element in this)
            {
                res += $"\n  {(++index).ToString(format)}) {element}";
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
