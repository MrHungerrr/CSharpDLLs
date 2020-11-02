using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Vkimow.Unity.GOAP.Cost;
using Vkimow.Structures.Trees.Decision;
using Vkimow.Tools.Math;


namespace Vkimow.Unity.GOAP
{
    public class GOAPPlaner
    {
        private readonly GOAPPlanBuilder _builder;
        private readonly IGOAPCostComparer _comparer;

        public GOAPPlaner(GOAPStateContext context, IGOAPCostComparer comparer)
        {
            _builder = new GOAPPlanBuilder(context);
            _comparer = comparer;
        }

        public bool TryGetPlan(KeyValuePair<string, GOAPState> goal, out List<GOAPAction> plan)
        {
            var plans = new TreeDecision<GOAPAction>();
            plan = null;

            if (!_builder.TryBuildPlans(goal, out plans))
                return false;

            plan = GetBestPlan(plans);
            return true;
        }

        private List<GOAPAction> GetBestPlan(TreeDecision<GOAPAction> plans)
        {
            var bestCost = _comparer.BadCost;
            var bestPlan = new List<GOAPAction>();
            var stack = new Stack<GOAPAction>();

            foreach (var child in plans.Root.Children)
            {
                Iterate(_comparer.ZeroCost, child);
            }

            void Iterate(IGOAPCost previusCost, TreeElement<GOAPAction> element)
            {
                var newCost = previusCost.GetSumWith(element.Content.Cost);
                stack.Push(element.Content);

                if (!element.HasChildren)
                {
                    int value = _comparer.Compare(bestCost, newCost);

                    if (value < 0 || (value == 0 && BaseMath.Probability(0.5f)))
                    {
                        bestCost = newCost;
                        bestPlan = stack.Where(x => !x.IsConnector).ToList();
                    }

                    return;
                }

                foreach (var child in element.Children)
                {
                    Iterate(newCost, child);
                }

                while (stack.Pop() != element.Content) ;
            }

            return bestPlan;
        }
    }
}
