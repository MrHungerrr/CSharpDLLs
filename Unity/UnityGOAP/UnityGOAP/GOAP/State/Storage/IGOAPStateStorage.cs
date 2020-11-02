using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Vkimow.Unity.GOAP
{
    public interface IGOAPStateStorage : IGOAPStateSingle, IGOAPStateReadOnlyStorage
    {
        void Add(string key, object value);
        void Add(string key, GOAPState state);
        void Add(KeyValuePair<string, GOAPState> state);
        void Remove(string key);
    }
}
