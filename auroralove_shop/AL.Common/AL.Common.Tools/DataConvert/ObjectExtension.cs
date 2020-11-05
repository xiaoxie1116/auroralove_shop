using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace AL.Common.Tools
{
    public static class ObjectExtension
    {
        #region 时间类型互转
        public static string ToTimeSpanStr(this TimeSpan? tsp)
        {
            if (tsp == null) return "00:00";
            TimeSpan ts = tsp.Value;
            string hour = ts.Hours < 10 ? "0" + ts.Hours : ts.Hours.ToString();
            string min = ts.Minutes < 10 ? "0" + ts.Minutes : ts.Minutes.ToString();
            return hour + ":" + min;
        }
        /// <summary>
        /// yyyy-MM-dd
        /// </summary>
        public static string ToShortDateStr(this DateTime? dateTime)
        {
            if (dateTime == null) return "";
            return String.Format("{0:yyyy-MM-dd}", dateTime.Value);
        }
        /// <summary>
        /// yyyy-MM-dd HH:mm
        /// </summary>
        public static string ToShortDateTimeStr(this DateTime? dateTime)
        {
            return String.Format("{0:yyyy-MM-dd HH:mm}", dateTime);
        }
        /// <summary>
        /// yyyy-MM-dd HH:mm:ss
        /// </summary>
        public static string ToLongDateTimeStr(this DateTime? dateTime)
        {
            return String.Format("{0:yyyy-MM-dd HH:mm:ss}", dateTime);
        }

        public static DateTime ConvertToDateTime(string dateTime, out bool result)
        {
            DateTime tempDateTime = new DateTime();

            result = DateTime.TryParse(dateTime, out tempDateTime);

            return tempDateTime;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static DateTime? ToNullableDateTime(this object item)
        {
            DateTime dt;
            if (item != null && DateTime.TryParse(item.ToString(), out dt))
            {
                return dt;
            }
            return null;
        }

        #endregion

        #region Int类型转换

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static int ToInt(this object item)
        {
            return ToInt(item, 0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static int ToInt(this object item, int defaultValue)
        {
            if (item != null)
            {
                int.TryParse(item.ToString(), out defaultValue);
            }

            return defaultValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static int? ToNullableInt(this object item)
        {
            int result = 0;
            if (item != null && int.TryParse(item.ToString(), out result))
            {
                return result;
            }
            return null;
        }

 
        #endregion

        #region 枚举

        /// <summary>
        /// 将枚举（其中的值）转化为string
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToValueString<T>(this T value) where T : struct
        {
            Type type = value.GetType();
            if (type.IsEnum)
            {
                FieldInfo fieldInfo = type.GetField("value__");
                if (fieldInfo != null)
                {
                    return string.Format("{0}", fieldInfo.GetValue(value));
                }
            }
            return null;
        }
        #endregion

        #region 集合

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static bool CollectionsIsNullOrEmpty<T>(this ICollection<T> list)
        {
            if (list == null || list.Count <= 0)
                return true;
            return false;
        }
        /// <summary>
        /// 集合非空
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collections"></param>
        /// <returns></returns>
        public static bool CollectionIsNotNullOrEmpty<T>(this List<T> collections)
        {
            if (collections != null && collections.Count > 0)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static bool EnumerableIsNullOrEmpty<T>(this IEnumerable<T> list)
        {
            if (list == null || !list.Any())
            {
                return true;
            }
            return false;
        }
        #endregion
    }
}
