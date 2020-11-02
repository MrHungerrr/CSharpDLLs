using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Vkimow.Unity.GOAP.Cost;
using Vkimow.Structures.Trees.Decision;

namespace Vkimow.Unity.GOAP
{
    internal class GOAPPlanBuilder
    {
        private GOAPStateContext _context;

        internal GOAPPlanBuilder(GOAPStateContext context)
        {
            _context = context;
        }

        internal bool TryBuildPlans(KeyValuePair<string, GOAPState> goal, out TreeDecision<GOAPAction> plans)
        {
            plans = BuildPlansToGoal(goal);

            if (plans == null || plans.IsEmpty)
                return false;
            else
                return true;
        }

        private TreeDecision<GOAPAction> BuildPlansToGoal(KeyValuePair<string, GOAPState> goal)
        {
            var tree = new TreeDecision<GOAPAction>();

            List<GOAPAction> needActions;

            if (!GOAPActionsManager.Instance.TryGetActionsWithEffect(goal, out needActions))
                return null;


            for (int i = 0; i < needActions.Count; i++)
            {
                var actionTree = BuildPlansToAction(needActions[i]);

                if (actionTree != null)
                    tree.AddToRoot(actionTree);
            }

            if (tree.IsEmpty)
                return null;

            return tree;
        }

        private TreeDecision<GOAPAction> BuildPlansToAction(GOAPAction action)
        {
            var tree = new TreeDecision<GOAPAction>();
            tree.AddToRoot(action);

            var needConditions = new List<KeyValuePair<string, GOAPState>>();

            foreach (var condition in action.Preconditions)
            {
                if (!_context.Contains(condition))
                    needConditions.Add(condition);
            }

            if (needConditions.Count == 0)
                return tree;

            foreach (var condition in needConditions)
            {
                TreeDecision<GOAPAction> conditionTree = BuildPlansToGoal(condition);

                if (conditionTree == null)
                    return null;

                tree.AddToLeafs(conditionTree);
            }

            return tree;
        }
    }
}
