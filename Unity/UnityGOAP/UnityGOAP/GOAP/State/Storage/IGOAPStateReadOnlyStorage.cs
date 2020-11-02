using System.Collections.Generic;

namespace Vkimow.Unity.GOAP
{
    public interface IGOAPStateReadOnlyStorage : IGOAPStateReadOnlySingle, IEnumerable<KeyValuePair<string, GOAPState>>
    {
        GOAPState this[string key] { get; }
    }
}
