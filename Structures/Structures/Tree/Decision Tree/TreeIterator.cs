using System;
using System.Linq;
using System.Collections.Generic;

namespace Vkimow.Structures.Trees.Decision
{
    public static class TreeIterator
    {

        /// <summary>
        /// Итерировать каждую ветку без повторов элементов
        /// </summary>
        public static IEnumerable<TreeElement<T>> BranchIterator<T>(this TreeDecision<T> tree)
        {
            if(tree.Root.HasChildren)
                return GetElementsDown(tree.Root);

            throw new Exception("Пустое дерево!");
        }

        internal static IEnumerable<TreeElement<T>> BranchIteratorWithRoot<T>(this TreeDecision<T> tree)
        {
            yield return tree.Root;

            if (tree.Root.HasChildren)
            {
                foreach (var element in GetElementsDown(tree.Root))
                {
                    yield return element;
                }
            }
        }

        /// <summary>
        /// Итерировать каждую ветку с повторами элементов
        /// </summary>
        public static IEnumerable<TreeElement<T>> BranchIteratorWithDuplicates<T>(this TreeDecision<T> tree)
        {
            if (tree.Root.HasChildren)
                return GetElementsDownWithDuplicates(tree.Root);

            throw new Exception("Пустое дерево!");
        }

        internal static IEnumerable<TreeElement<T>> GetElementsDown<T>(TreeElement<T> element)
        {
            var elementsWeFound = new HashSet<TreeElement<T>>();
            return GetElementsDown(element, elementsWeFound);
        }

        private static IEnumerable<TreeElement<T>> GetElementsDown<T>(TreeElement<T> element, HashSet<TreeElement<T>> elementsWeFound)
        {
            foreach (TreeElement<T> child in element.Children)
            {
                if (!elementsWeFound.Contains(child))
                {
                    yield return child;
                    elementsWeFound.Add(child);

                    if (child.HasChildren)
                    {
                        foreach (TreeElement<T> grandchild in GetElementsDown(child, elementsWeFound))
                        {
                            yield return grandchild;
                        }
                    }
                }
            }
        }

        private static IEnumerable<TreeElement<T>> GetElementsDownWithDuplicates<T>(TreeElement<T> element)
        {
            foreach (TreeElement<T> child in element.Children)
            {
                yield return child;

                if (child.HasChildren)
                {
                    foreach (TreeElement<T> grandchild in GetElementsDownWithDuplicates(child))
                    {
                        yield return grandchild;
                    }
                }
            }
        }


        /// <summary>
        /// Итерировать в форме змейки без повтором элементов
        /// </summary>
        public static IEnumerable<TreeElement<T>> SnakeIterator<T>(this TreeDecision<T> tree)
        {
            if (tree.Root.HasChildren)
                return GetElementsSnake(tree.Root.Children);

            throw new Exception("Пустое дерево!");
        }


        private static IEnumerable<TreeElement<T>> GetElementsSnake<T>(IEnumerable<TreeElement<T>> elements)
        {
            var children = new List<TreeElement<T>>();

            foreach (TreeElement<T> element in elements)
            {
                yield return element;

                if (element.HasChildren)
                    children.Union(element.Children);
            }

            foreach(TreeElement<T> grandchild in GetElementsSnake(children))
            {
                yield return grandchild;
            }
        }
    }
}
