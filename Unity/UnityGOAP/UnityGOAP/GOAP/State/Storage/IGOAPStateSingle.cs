using Serializators.XML;
using System;
using System.Collections.Generic;

namespace Vkimow.Unity.GOAP
{
    public interface IGOAPStateSingle : IGOAPStateReadOnlySingle, IXMLSerializable
    {
        void Clear();
        void Set(string key, object value);
        void Set(string key, GOAPState state);
        void Set(KeyValuePair<string, GOAPState> state);
    }
}
