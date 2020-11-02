using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Vkimow.Serializators.XML
{
    public static class XMLSerializator
    {
        private static string _nameOfList = "List";

        public static void Save(IXMLSerializable xml, string path)
        {
            var xDocument = new XDocument(new XDeclaration("1,0", "utf-8", "yes"), xml.ConvertToXML());
            xDocument.Save(path);
        }

        public static void Save(IXMLSerializable[] array, string path)
        {
            var xml = new XElement(_nameOfList);

            for(int i = 0; i < array.Length; i++)
            {
                xml.Add(array[i].ConvertToXML());
            }

            var xDocument = new XDocument(new XDeclaration("1,0", "utf-8", "yes"), xml);
            xDocument.Save(path);
        }

        public static void Save(List<IXMLSerializable> list, string path)
        {
            var xml = new XElement(_nameOfList);

            for (int i = 0; i < list.Count; i++)
            {
                xml.Add(list[i].ConvertToXML());
            }

            var xDocument = new XDocument(new XDeclaration("1,0", "utf-8", "yes"), xml);
            xDocument.Save(path);
        }

        public static void Save(IEnumerable<IXMLSerializable> list, string path)
        {
            var xml = new XElement(_nameOfList);

            foreach (var element in list)
            {
                xml.Add(element.ConvertToXML());
            }

            var xDocument = new XDocument(new XDeclaration("1,0", "utf-8", "yes"), xml);
            xDocument.Save(path);
        }



        public static T Load<T>(string path) where T : IXMLSerializable, new()
        {
            var element = new T();
            var xDocument = XDocument.Load(path);
            element.ReadXML(xDocument.Root);
            return element;
        }


        public static List<T> LoadList<T>(string path) where T : IXMLSerializable, new()
        {
            var xDocument = XDocument.Load(path);
            var list = new List<T>();

            var xElements = xDocument.Root.Elements();

            foreach(var xElement in xElements)
            {
                var element = new T();
                element.ReadXML(xElement);
                list.Add(element);
            }

            return list;
        }
    }
}
