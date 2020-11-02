using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Structures.Trees.Base.XML
{
    public static class TreeBuilder
    {

        private static XElement XMLBuild<T>(Tree<T> tree, Converter<T, XElement[]> converter)
        {
            int index = 0;
            var dictionary = new Dictionary<TreeElement<T>, int>();

            var xTree = new XElement("Tree");

            foreach (TreeElement<T> element in tree)
            {
                int parentIndex = -1;

                if (element.Parent != tree.Root)
                    parentIndex = dictionary[element.Parent];

                xTree.Add(new XElement("Element", new XAttribute("Id", index),
                                                 new XElement("Parent", parentIndex),
                                                 new XElement("Content",
                                                 converter(element.Content))));
                                                    
                dictionary.Add(element, index++);
            }

            return xTree;
        }
    }
}
