using System;
using System.Collections.Generic;
using System.Text;

namespace AL.Common.Tools.Util
{
    public static class ListUtil
    {
        public static void RandomSort<T>(this List<T> s)
        {
            int len = s.Count;
            Random ram = new Random();
            int currentIndex;
            T tempValue;

            for (int i = 0; i < len; i++)
            {
                currentIndex = ram.Next(0, len - i);
                tempValue = s[currentIndex];
                s[currentIndex] = s[len - i - 1];
                s[len - 1 - i] = tempValue;
            }
        }
        public static List<int> ToIntList(this IEnumerable<object> oList)
        {
            List<int> iList = new List<int>();
            foreach (object o in oList)
            {
                iList.Add(Convert.ToInt32(o));
            }
            return iList;
        }

        /// <summary>
        /// 判断List<T> 非空 且Count>0
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static bool GetSafeList<T>(this List<T> list)
        {
            if (list != null && list.Count > 0)
                return true;
            return false;
        }
    }
}
