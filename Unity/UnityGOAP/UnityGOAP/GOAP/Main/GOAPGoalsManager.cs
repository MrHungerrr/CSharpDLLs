using Serializators.XML;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using Vkimow.Tools.Single;

namespace Vkimow.Unity.GOAP
{
    public class GOAPGoalsManager: Singleton<GOAPGoalsManager>, IXMLSerializable
    {
        public IReadOnlyDictionary<string, KeyValuePair<string, GOAPState>> Goals => _goals;

        private Dictionary<string, KeyValuePair<string, GOAPState>> _goals;

        public GOAPGoalsManager()
        {
            _goals = new Dictionary<string, KeyValuePair<string, GOAPState>>();
        }

        public void Add(string goalKey, string stateKey, object stateValue)
        {
            _goals.Add(goalKey, new KeyValuePair<string, GOAPState>(stateKey, new GOAPState(stateValue)));
        }

        public void Clear()
        {
            _goals.Clear();
        }




        #region XML Serialization

        public XElement ConvertToXML()
        {
            return ConvertToXML("GOAPGoals");
        }

        public XElement ConvertToXML(string name)
        {
            var xElement = new XElement(name);

            foreach (var goal in _goals)
            {
                var xKeyValue = new XElement("Goal", new XAttribute("GoalKey", goal.Key), new XElement("Key", goal.Value.Key), new XElement("Value", goal.Value.Value.Value));
                xElement.Add(xKeyValue);
            }

            return xElement;
        }

        public void ReadXML(XElement xMainElement)
        {
            foreach (var xElement in xMainElement.Elements())
            {
                var goalKey = (string)xElement.Attribute("GoalKey");
                var stateKey = (string)xElement.Element("Key");
                object value;

                switch (GOAPBlanksManager.Instance.Blanks[stateKey].ToString())
                {
                    case "System.Boolean":
                        {
                            value = (bool)xElement.Element("Value");
                            break;
                        }
                    case "System.String":
                        {
                            value = (string)xElement.Element("Value");
                            break;
                        }
                    case "System.Int32":
                        {
                            value = (int)xElement.Element("Value");
                            break;
                        }
                    default:
                        {
                            throw new InvalidCastException();
                        }
                }

                Add(goalKey, stateKey, value);
            }
        }

        #endregion
    }
}
