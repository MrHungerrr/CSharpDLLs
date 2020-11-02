using System;
using System.Collections.Generic;

namespace Vkimow.Structures.Trees.Base
{
    public static class TreeIterator
    {
        /// <summary>
        /// Итерировать каждую ветку с корневым элементом
        /// </summary>
        public static IEnumerable<TreeElement<T>> BranchIteratorWithRoot<T>(this Tree<T> tree)
        {
            yield return tree.Root;

            if (!tree.IsEmpty)
            {
                foreach (TreeElement<T> element in GetElementsDown(tree.Root))
                {
                    yield return element;
                }
            }
        }

        /// <summary>
        /// Итерировать каждую ветку
        /// </summary>
        public static IEnumerable<TreeElement<T>> BranchIterator<T>(this Tree<T> tree)
        {
            if (!tree.Root.HasChildren)
                throw new Exception();

            return GetElementsDown(tree.Root);
        }


        internal static IEnumerable<TreeElement<T>> GetElementsDown<T>(TreeElement<T> element)
        {
            foreach (TreeElement<T> child in element.Children)
            {
                yield return child;

                if (child.HasChildren)
                {
                    foreach (TreeElement<T> grandchild in GetElementsDown(child))
                    {
                        yield return grandchild;
                    }
                }
            }
        }


        /// <summary>
        /// Итерировать в форме змейки
        /// </summary>
        public static IEnumerable<TreeElement<T>> SnakeIterator<T>(this Tree<T> tree)
        {
            return GetElementsSnake(tree.Root.Children);
        }


        /// <summary>
        /// Итерировать в форме змейки с корневым элементом
        /// </summary>
        public static IEnumerable<TreeElement<T>> SnakeIteratorWithRoot<T>(this Tree<T> tree)
        {
            return GetElementsSnake(new List<TreeElement<T>>() { tree.Root});
        }


        private static IEnumerable<TreeElement<T>> GetElementsSnake<T>(IEnumerable<TreeElement<T>> elements)
        {
            var children = new List<TreeElement<T>>();

            foreach (TreeElement<T> element in elements)
            {
                yield return element;

                if (element.HasChildren)
                    children.AddRange(element.Children);
            }

            foreach(TreeElement<T> child in GetElementsSnake(children))
            {
                yield return child;
            }
        }
    }
}
