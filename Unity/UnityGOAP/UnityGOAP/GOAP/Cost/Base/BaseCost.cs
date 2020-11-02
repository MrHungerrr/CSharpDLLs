using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Xml.Linq;

namespace Vkimow.Unity.GOAP.Cost
{
    public struct BaseCost : IGOAPCost, IComparable<BaseCost>
    {
        public int Cost => _cost;

        private int _cost;


        public BaseCost(int cost)
        {
            _cost = cost;
        }

        public void Add(IGOAPCost cost)
        {
            if (!(cost is BaseCost))
                throw new InvalidCastException();

            Add((BaseCost)cost);
        }

        private void Add(BaseCost other)
        {
            _cost += other._cost;
        }


        public IGOAPCost GetSumWith(IGOAPCost other)
        {
            if (!(other is BaseCost))
                throw new InvalidCastException();

            return new BaseCost(_cost + ((BaseCost)other)._cost);
        }


        public int CompareTo(IGOAPCost other)
        {
            if (other == null)
                throw new NullReferenceException();

            if (!(other is BaseCost))
                throw new InvalidCastException();

            return CompareTo((BaseCost)other);
        }

        public int CompareTo(BaseCost other)
        {
            return _cost.CompareTo(other._cost);
        }
        public override string ToString()
        {
            return $"{_cost}";
        }


        #region XML Serialization

        public XElement ConvertToXML()
        {
            return ConvertToXML("BaseCost");
        }

        public XElement ConvertToXML(string name)
        {
            var xElement = new XElement(name, new XAttribute("Type", "BaseCost"), new XElement("Cost", _cost));
            return xElement;
        }

        public void ReadXML(XElement xElement)
        {
            _cost = (int)xElement.Element("Cost");
        }

        #endregion
    }
}
