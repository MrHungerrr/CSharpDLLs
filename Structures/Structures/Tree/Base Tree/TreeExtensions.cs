using System;

namespace Vkimow.Structures.Trees.Base
{
    public static class TreeExtensions
    {
        public static Tree<Toutput> Convert<Tinput, Toutput>(this Tree<Tinput> treeIn, Converter<Tinput, Toutput> converter)
        {
            var treeOut = new Tree<Toutput>();

            Iterate(treeIn.Root, treeOut.Root);

            void Iterate(TreeElement<Tinput> elementIn, TreeElement<Toutput> elementOut)
            {
                if (elementIn.HasChildren)
                {
                    foreach (var childElement in elementIn)
                    {
                        treeOut.Add(converter(childElement.Content), elementOut);
                        Iterate(childElement, treeOut.LastElement);
                    }
                }
            }

            return treeOut;
        }


        public static Tree<T> Copy<T>(this Tree<T> tree) where T : ICloneable
        {
            return Convert<T, T>(tree, x => (T)x.Clone()) ;
        }

        public static Tree<T> CopyStruct<T>(this Tree<T> tree) where T: struct
        {
            return Convert<T, T>(tree, x => x);
        }
    }
}
