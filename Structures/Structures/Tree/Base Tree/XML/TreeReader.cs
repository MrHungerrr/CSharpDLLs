using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Structures.Trees.Base.XML
{
    public static class TreeReader
    {
        /// <summary>
        /// Певращает связку {Id элемента, {ID Родителя, Содержимое}} В дерево
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tree">Связка {Id элемента, {ID Родителя, Содержимое}}</param>
        /// <param name="parent"></param>
        /// <returns></returns>
        public static Tree<T> BuildTree<T>(Dictionary<int, Tuple<int, T>> tree, int parent = -1)
        {
            List<int> childs = tree
                .Where(x => x.Value.Item1 == parent)
                .Select(x => x.Key).ToList();

            if (childs.Count > 0)
            {
                var treeBuf = new Tree<T>();

                foreach (var child in childs)
                {
                    treeBuf.AddToRoot(tree[child].Item2);
                    var childElement = treeBuf.LastElement;

                    var treeOfChild = BuildTree(tree, child);
                    if (treeOfChild != null)
                    {
                        treeBuf.Add(treeOfChild, childElement);
                    }
                }
                return treeBuf;
            }
            else
            {
                return null;
            }
        }

        private static Tree<T> Load<T>(XElement treeContainer, Converter<XElement, T> converter)
        {
            var treeBuf = from element in treeContainer.Elements()
                          select new
                          {
                              Id = (int)element.Attribute("Id"),
                              Parent = (int)element.Element("Parent"),
                              Content = element.Element("Content"),
                          };

            var tree = treeBuf.ToDictionary(x => x.Id,
                x => new Tuple<int, T>(x.Parent, converter(x.Content)));
      
            return BuildTree(tree); ;
        }
    }
}
