using Serializators.XML;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Vkimow.Unity.GOAP.Cost
{
    public interface IGOAPCost : IComparable<IGOAPCost>, IXMLSerializable
    {
        void Add(IGOAPCost other);
        IGOAPCost GetSumWith(IGOAPCost other);
        string ToString();
    }
}
