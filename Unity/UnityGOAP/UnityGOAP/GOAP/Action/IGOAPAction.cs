using Serializators.XML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vkimow.Unity.GOAP.Cost;

namespace Vkimow.Unity.GOAP
{
    public interface IGOAPAction : IGOAPReadOnlyAction, IXMLSerializable
    {
        void SetName(string name);
        IGOAPStateStorage GetPreconditions();
        IGOAPStateSingle GetEffect();
        void SetCost(IGOAPCost cost);
    }
}
