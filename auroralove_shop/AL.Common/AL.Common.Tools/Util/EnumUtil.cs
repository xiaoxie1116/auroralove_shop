using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;

namespace AL.Common.Tools.Util
{
    /// <summary>
    /// 枚举数据静态类
    /// </summary>
    public static class EnumUtil
    {
        private static void TestGetDescription()
        {
            System.DateTimeKind dtk1 = DateTimeKind.Local;
            string s1 = dtk1.GetDescription();



            System.DateTimeKind dtk2 = DateTimeKind.Unspecified;

            string s2 = dtk2.GetDescription();

            System.DateTimeKind dtk3 = DateTimeKind.Local;

            string s3 = dtk3.GetDescription();

        }
        private static SortedList<Enum, string> enumDesciptionDict = new SortedList<Enum, string>();
        public static string GetDescription(this Enum value)
        {
            string description = string.Empty;
            if (enumDesciptionDict.TryGetValue(value, out description)) return description;

            FieldInfo field = value.GetType().GetField(value.ToString());
            DescriptionAttribute attribute = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;
            if (attribute == null)
                enumDesciptionDict[value] = value.ToString();
            else
                enumDesciptionDict[value] = attribute.Description;
            return enumDesciptionDict[value];
        }

        public static DescriptionAttribute GetAttribute(this Enum value)
        {
            FieldInfo field = value.GetType().GetField(value.ToString());
            return Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;
        }


        public static Dictionary<int, string> GetEnumIntNameDict(Enum e)
        {
            return GetEnumNameValueDict(e, (Object o) => { return Convert.ToInt32(o); });
        }

        public static Dictionary<TOutput, string> GetEnumNameValueDict<TOutput>(Enum e, Converter<Object, TOutput> convert)
        {
            Type enumType = e.GetType();
            if (!enumType.IsEnum)
            {
                throw new ArgumentException("参数必须是枚举类型", "enumType");
            }

            FieldInfo[] fields = enumType.GetFields();

            if (fields != null)
            {
                Dictionary<TOutput, string> dict = new Dictionary<TOutput, string>();

                foreach (FieldInfo f in fields)
                {
                    if (f.Name.EndsWith("__")) continue;

                    DescriptionAttribute[] attrs = (DescriptionAttribute[])f.GetCustomAttributes(typeof(DescriptionAttribute), false);
                    string name = f.Name;
                    if (attrs != null && attrs.Length == 1)
                    {
                        name = attrs[0].Description;
                    }
                    TOutput val = convert(f.GetValue(e));
                    dict.Add(val, name);
                }
                return dict;
            }

            return null;
        }

        public static Dictionary<int, string> GetEnumWithDataMemberIntNameDict(Enum e)
        {
            Type enumType = e.GetType();
            if (!enumType.IsEnum)
            {
                throw new ArgumentException("参数必须是枚举类型", "enumType");
            }

            FieldInfo[] fields = enumType.GetFields();

            if (fields != null)
            {
                Dictionary<int, string> dict = new Dictionary<int, string>();

                foreach (FieldInfo f in fields)
                {
                    if (f.Name.EndsWith("__")) continue;
                    EnumMemberAttribute[] dataMemberAttrs = (EnumMemberAttribute[])f.GetCustomAttributes(typeof(EnumMemberAttribute), false);
                    if (dataMemberAttrs != null && dataMemberAttrs.Length == 1)
                    {
                        DescriptionAttribute[] attrs = (DescriptionAttribute[])f.GetCustomAttributes(typeof(DescriptionAttribute), false);
                        string name = f.Name;
                        if (attrs != null && attrs.Length == 1)
                        {
                            name = attrs[0].Description;
                        }
                        int val = (int)f.GetValue(e);
                        dict.Add(val, name);
                    }
                }
                return dict;
            }

            return null;
        }

        public static Dictionary<int, string> GetEnumNoDataMemberIntNameDict(Enum e)
        {
            Type enumType = e.GetType();
            if (!enumType.IsEnum)
            {
                throw new ArgumentException("参数必须是枚举类型", "enumType");
            }

            FieldInfo[] fields = enumType.GetFields();

            if (fields != null)
            {
                Dictionary<int, string> dict = new Dictionary<int, string>();

                foreach (FieldInfo f in fields)
                {
                    if (f.Name.EndsWith("__")) continue;
                    EnumMemberAttribute[] dataMemberAttrs = (EnumMemberAttribute[])f.GetCustomAttributes(typeof(EnumMemberAttribute), false);
                    if (dataMemberAttrs == null || dataMemberAttrs.Length == 0)
                    {
                        DescriptionAttribute[] attrs = (DescriptionAttribute[])f.GetCustomAttributes(typeof(DescriptionAttribute), false);
                        string name = f.Name;
                        if (attrs != null && attrs.Length == 1)
                        {
                            name = attrs[0].Description;
                        }
                        int val = (int)f.GetValue(e);
                        dict.Add(val, name);
                    }
                }
                return dict;
            }

            return null;
        }

        public static Dictionary<string, string> GetEnumVarNameDict(Enum e)
        {
            Type enumType = e.GetType();
            if (!enumType.IsEnum)
            {
                throw new ArgumentException("参数必须是枚举类型", "enumType");
            }

            FieldInfo[] fields = enumType.GetFields();

            if (fields != null)
            {
                Dictionary<string, string> dict = new Dictionary<string, string>();

                foreach (FieldInfo f in fields)
                {
                    if (f.Name.EndsWith("__")) continue;

                    DescriptionAttribute[] attrs = (DescriptionAttribute[])f.GetCustomAttributes(typeof(DescriptionAttribute), false);
                    string name = f.Name;
                    if (attrs != null && attrs.Length == 1)
                    {
                        name = attrs[0].Description;
                    }
                    dict.Add(f.Name, name);
                }
                return dict;
            }

            return null;
        }



        public static string GetEnumDescription(Enum e)
        {
            string ret = string.Empty;
            Type enumType = e.GetType();
            if (!enumType.IsEnum)
            {
                throw new ArgumentException("参数必须是枚举类型", "enumType");
            }
            MemberInfo[] members = enumType.GetMember(e.ToString());
            if (members != null && members.Length == 1)
            {
                DescriptionAttribute[] attrs = members[0].GetCustomAttributes(typeof(DescriptionAttribute), false) as DescriptionAttribute[];
                if (attrs.Length == 1)
                {
                    return attrs[0].Description;
                }
            }
            return e.ToString();
        }


        #region 枚举数据方法
        /**/
        /// <summary>
        /// 从Enum中任意取一个Int值，将其转化成枚举类型值
        /// </summary>
        /// <param name="protocolType"></param>
        /// <param name="enumValue"></param>
        /// <returns></returns>
        /// <example>ExampleNormalEnum status  = (ExampleNormalEnum)EnumHelper.IntValueToEnum( typeof( ExampleNormalEnum ), 1); 得到值为 ExampleNormalEnum.Online </example>
        public static object IntValueToEnum(System.Type enumType, int enumIntValue)
        {
            try
            {
                if (!enumType.IsEnum)
                {
                    throw new ArgumentException("参数必须是枚举类型", "enumType");
                }

                object myObject = Enum.Parse(enumType, Enum.GetName(enumType, enumIntValue));
                return myObject;
            }
            catch
            {
                return (object)null;
            }
        }

        /// <summary>
        /// Enums the value to int.
        /// </summary>
        /// <param name="protocolType">Type of the protocol.</param>
        /// <param name="enumValue">The enum value.</param>
        /// <returns></returns>
        public static int EnumValueToInt(System.Type enumType, object enumValue)
        {
            if (!enumType.IsEnum)
            {
                throw new ArgumentException("参数必须是枚举类型", "enumType");
            }
            int[] myIntArray = GetEnumIntValues(enumType);
            foreach (int Key in myIntArray)
            {
                if (IntValueToEnum(enumType, Key).Equals(enumValue))
                    return Key;
            }
            return -1;
        }


        /**/
        /// <summary>
        /// 从Enum中任意取一个String值，将其转化成枚举类型值
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="enumStringValue"></param>
        /// <returns></returns>
        /// <example>ExampleNormalEnum status  = (ExampleNormalEnum)EnumHelper.StringValueToEnum( typeof( ExampleNormalEnum ), "Offline");得到值为 ExampleNormalEnum.Offline</example>
        public static object StringValueToEnum(System.Type enumType, string enumStringValue)
        {
            if (!enumType.IsEnum)
            {
                throw new ArgumentException("参数必须是枚举类型", "enumType");
            }

            object myObject = Enum.Parse(enumType, enumStringValue, true);
            return myObject;
        }


        /**/
        /// <summary>
        /// 得到一个Enum中的所有Int值
        /// </summary>
        /// <param name="protocolType"></param>
        /// <returns></returns>
        public static int[] GetEnumIntValues(System.Type enumType)
        {
            if (!enumType.IsEnum)
            {
                throw new ArgumentException("参数必须是枚举类型", "enumType");
            }
            Array array = Enum.GetValues(enumType);
            int[] myIntArray = new int[array.Length];
            Array.Copy(array, myIntArray, array.Length);

            return myIntArray;
        }
        #endregion
    }
}
