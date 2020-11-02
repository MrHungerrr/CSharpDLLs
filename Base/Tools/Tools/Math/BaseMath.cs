using System;
using System.Collections.Generic;
using System.Text;

namespace Vkimow.Tools.Math
{
    public static class BaseMath
    {
        public static bool Probability(double value)
        {
            var random = new Random();
            var chance = random.NextDouble();

            if(chance <= value)
            {
                return true;
            }

            return false;
        }
    }
}
