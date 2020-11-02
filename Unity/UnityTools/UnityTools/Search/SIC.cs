using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vkimow.Unity.Tools.Search
{
    /// <summary>
    /// Search In Children by KEY
    /// </summary>
    public static class SIC
    {
        //Поиск компонентов
        public static Transform[] Components(Transform obj, string tag)
        {
            List<Transform> list = new List<Transform>();

            if (obj.tag == tag)
            {
                list.Add(obj);
            }

            for (int i = 0; i < obj.childCount; i++)
            {
                list.AddRange(Components(obj.GetChild(i), tag));
            }

            return list.ToArray();
        }

        //Перегрузки для GameObjects Вместо Transform
        public static void Components(GameObject obj, string tag, out Transform[] array)
        {
            array = Components(obj.transform, tag);
        }

        public static void Components(GameObject obj, string tag, out GameObject[] array)
        {
            Transform[] buf_array = Components(obj.transform, tag);
            List<GameObject> list = new List<GameObject>();

            foreach (Transform t in buf_array)
            {
                list.Add(t.gameObject);
            }

            array = list.ToArray();
        }






        //Поиск компонента
        public static Transform Component(Transform obj, string key)
        {
            if (obj.name == key)
            {
                return obj;
            }

            for (int i = 0; i < obj.childCount; i++)
            {
                Transform buf = Component(obj.GetChild(i), key);
                if (buf != null)
                {
                    return buf;
                }
            }

            return null;
        }




        //Перегрузки для GameObjects Вместо Transform
        public static void Component(GameObject obj, string key, out GameObject goal)
        {
            goal = Component(obj.transform, key).gameObject;
        }

        public static void Component(GameObject obj, string key, out Transform goal)
        {
            goal = Component(obj.transform, key);
        }
    }
}
