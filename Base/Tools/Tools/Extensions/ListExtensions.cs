using System;
using System.Collections.Generic;
using System.Text;

namespace Vkimow.Tools.Extensions
{
    public static class ListExtensions
    {
        public static void Shuffle<T>(this List<T> list)
        {
            var random = new Random();

            for (int i = list.Count - 1; i > 0; i--)
            {
                var j = random.Next(i + 1);

                var tmp = list[j];
                list[j] = list[i];
                list[i] = tmp;
            }
        }

        public static T GetRandomElement<T>(this List<T> list)
        {
            var random = new Random();
            var index = random.Next(0, list.Count);
            return list[index];
        }
    }
}
