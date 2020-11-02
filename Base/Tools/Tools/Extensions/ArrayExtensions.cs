using System;
using System.Collections.Generic;
using System.Text;

namespace Vkimow.Tools.Extensions
{
    public static class ArrayExtensions
    {
        public static void Shuffle<T>(this T[] array)
        {
            var random = new Random();

            for (int i = array.Length - 1; i > 0; i--)
            {
                var j = random.Next(i + 1);

                var tmp = array[j];
                array[j] = array[i];
                array[i] = tmp;
            }
        }

        public static T GetRandomElement<T>(this T[] array)
        {
            var random = new Random();
            var index = random.Next(0, array.Length);
            return array[index];
        }
    }
}
