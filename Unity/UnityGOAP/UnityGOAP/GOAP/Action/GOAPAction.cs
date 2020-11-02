using Vkimow.Unity.GOAP.Cost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Vkimow.Unity.GOAP
{
    public sealed class GOAPAction : IGOAPAction
    {
        public string Name { get; private set; }
        public bool IsConnector { get; private set; }
        public IGOAPStateReadOnlySingle Effect => _effect;
        public IGOAPStateReadOnlyStorage Preconditions => _preconditions;
        public IGOAPCost Cost { get; private set; }


        private IGOAPStateStorage _preconditions;
        private IGOAPStateSingle _effect;


        public GOAPAction(string action , bool isConnector)
        {
            Name = action;
            IsConnector = isConnector;
            _preconditions = new GOAPStateStorage();
            _effect = new GOAPStateSingle();
        }

        public GOAPAction(string action)
        {
            Name = action;
            IsConnector = false;
            _preconditions = new GOAPStateStorage();
            _effect = new GOAPStateSingle();
        }

        public void SetName(string name)
        {
            Name = name;
        }

        public IGOAPStateSingle GetEffect()
        {
            return _effect;
        }

        public IGOAPStateStorage GetPreconditions()
        {
            return _preconditions;
        }

        public void SetCost(IGOAPCost cost)
        {
            Cost = cost;
        }

        public XElement ConvertToXML()
        {
            return ConvertToXML("Action");
        }

        public XElement ConvertToXML(string name)
        {
            var xElement = new XElement(name,
                new XAttribute("Name", Name),
                new XAttribute("IsConnector", IsConnector));

            xElement.Add(Cost.ConvertToXML("Cost"));
            xElement.Add(_effect.ConvertToXML("Effect"));
            xElement.Add(_preconditions.ConvertToXML("Preconditions"));

            return xElement;
        }

        public void ReadXML(XElement xElement)
        {
            Name = (string)xElement.Attribute("Name");
            IsConnector = (bool)xElement.Attribute("IsConnector");
            Cost = GOAPCostSerializer.Instance.Deserialize(xElement.Element("Cost"));
            _effect.ReadXML(xElement.Element("Effect"));
            _preconditions.ReadXML(xElement.Element("Preconditions"));
        }

        public override string ToString()
        {
            if (!IsConnector)
                return ($"Action: \"{Name}\"");
            else
                return ($"Connector: \"{Name}\"");
        }
    }
}
