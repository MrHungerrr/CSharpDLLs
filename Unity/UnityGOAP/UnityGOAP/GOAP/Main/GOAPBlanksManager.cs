using Serializators.XML;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using Vkimow.Tools.Single;

namespace Vkimow.Unity.GOAP
{
    public class GOAPBlanksManager : Singleton<GOAPBlanksManager>, IXMLSerializable
    {
        public IReadOnlyDictionary<string, Type> Blanks => _blanks;

        private Dictionary<string, Type> _blanks;

        public GOAPBlanksManager()
        {
            _blanks = new Dictionary<string, Type>();
        }

        public void Add(string key, Type type)
        {
            _blanks.Add(key, type);
        }

        public void Clear()
        {
            _blanks = new Dictionary<string, Type>();
        }

        internal bool Contains(string key)
        {
            return _blanks.ContainsKey(key);
        }

        internal Type GetTypeOf(string key)
        {
            return _blanks[key];
        }


        #region XML Serialization
        public XElement ConvertToXML()
        {
            return ConvertToXML("GOAPBlanks");
        }

        public XElement ConvertToXML(string name)
        {
            var xElement = new XElement(name);

            foreach (var blank in _blanks)
            {
                var xKeyValue = new XElement("Blank", new XElement("Key", blank.Key), new XElement("Type", blank.Value.ToString()));
                xElement.Add(xKeyValue);
            }

            return xElement;
        }

        public void ReadXML(XElement xMainElement)
        {
            foreach (var xElement in xMainElement.Elements())
            {
                var key = (string)xElement.Element("Key");
                var type = Type.GetType((string) xElement.Element("Type"));
                Add(key, type);
            }
        }
        #endregion
    }
}
