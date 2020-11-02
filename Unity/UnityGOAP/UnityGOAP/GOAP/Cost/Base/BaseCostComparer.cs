using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Vkimow.Unity.GOAP.Cost
{
    public class BaseCostComparer : IComparer<BaseCost> , IGOAPCostComparer
    {
        public IGOAPCost ZeroCost => new BaseCost(0);
        public IGOAPCost BadCost => new BaseCost(0);

        private readonly int _threshold;
        private Comparison<BaseCost> _comparation;

        public BaseCostComparer(int threshold = Int32.MaxValue)
        {
            _threshold = threshold;
            _comparation = Comparsion;
        }

        public BaseCostComparer(Comparison<BaseCost> comparsion, int threshold = Int32.MaxValue)
        {
            _threshold = threshold;
            _comparation = comparsion ?? Comparsion;
        }

        public int Compare(IGOAPCost x, IGOAPCost y)
        {
            if (!(x is BaseCost) || !(y is BaseCost))
                throw new InvalidCastException();

            return Compare((BaseCost)x, (BaseCost)y);
        }

        public int Compare(BaseCost x, BaseCost y)
        {
            return _comparation(x, y);
        }

        private int Comparsion(BaseCost x, BaseCost y)
        {
            var xNotValid = !ValueIsValid(x);
            var yNotValid = !ValueIsValid(y);


            if (yNotValid || xNotValid)
            {
                if (yNotValid && xNotValid)
                    throw new Exception("Оба значения находятся за пределом!");

                if (yNotValid)
                    return 1;

                if (xNotValid)
                    return -1;
            }

            return x.CompareTo(y);
        }

        private bool ValueIsValid(BaseCost cost)
        {
            return  cost.Cost <= _threshold;   
        }
    }
}
