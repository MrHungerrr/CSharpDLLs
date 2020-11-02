using System;
using System.Collections.Generic;
using UnityEngine;

namespace Vkimow.Unity.Tools.Search
{
    /// <summary>
    /// Search In Children By <T>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static class SIC<T> where T : MonoBehaviour
    { 

        //Поиск компонентов
        public static T[] Components(Transform obj)
        {
            List<T> list = new List<T>();
            T buf;

            if (obj.TryGetComponent<T>(out buf))
            {
                list.Add(buf);
            }

            for (int i = 0; i < obj.childCount; i++)
            {
                list.AddRange(Components(obj.GetChild(i)));
            }

            return list.ToArray();
        }


        public static T[] ComponentsDown(Transform obj)
        {
            List<T> list = new List<T>();

            for (int i = 0; i < obj.childCount; i++)
            {
                list.AddRange(Components(obj.GetChild(i)));
            }

            return list.ToArray();
        }


        //Перегрузки для GameObjects Вместо Transform
        public static void Components(GameObject obj, out T[] array)
        {
            array = Components(obj.transform);
        }

        public static void Components(GameObject obj, out GameObject[] array)
        {
            T[] buf_array = Components(obj.transform);
            List<GameObject> list = new List<GameObject>();

            for (int i = 0; i < buf_array.Length; i++)
            {
                list.Add(buf_array[i].gameObject);
            }

            array = list.ToArray();
        }






        //Поиск компонента
        public static T Component(Transform obj)
        {
            T buf;

            if (obj.TryGetComponent<T>(out buf))
            {
                return buf;
            }

            for (int i = 0; i < obj.childCount; i++)
            {
                buf = Component(obj.GetChild(i));
                if (buf != default)
                {
                    return buf;
                }
            }

            return default(T);
        }

        public static T ComponentDown(Transform obj)
        {
            T buf;

            for (int i = 0; i < obj.childCount; i++)
            {
                buf = Component(obj.GetChild(i));
                if (buf != null)
                {
                    return buf;
                }
            }

            return default;
        }


        public static T Component(Transform obj, string key)
        {
            T buf;

            if (obj.name == key  && obj.TryGetComponent<T>(out buf))
            {
                return buf;
            }

            for (int i = 0; i < obj.childCount; i++)
            {
                buf = Component(obj.GetChild(i), key);
                if (buf != default(T))
                {
                    return buf;
                }
            }

            return default(T);
        }



        //Перегрузки для GameObjects Вместо Transform
        public static void Component(GameObject obj, out T goal)
        {
            goal = Component(obj.transform);
        }

        public static void Component(GameObject obj, out GameObject goal)
        {
            goal = Component(obj.transform).gameObject;
        }


    }
}
