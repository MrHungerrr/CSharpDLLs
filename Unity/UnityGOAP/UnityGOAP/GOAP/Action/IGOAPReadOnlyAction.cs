using System;
using Vkimow.Unity.GOAP.Cost;

namespace Vkimow.Unity.GOAP
{
    public interface IGOAPReadOnlyAction
    {
        string Name { get; }
        IGOAPStateReadOnlySingle Effect { get; }
        IGOAPStateReadOnlyStorage Preconditions { get; }
        IGOAPCost Cost { get; }
        string ToString();
    }
}
