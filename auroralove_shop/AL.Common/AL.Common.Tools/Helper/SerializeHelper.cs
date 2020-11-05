using System;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace AL.Common.Tools.Helper
{
    /// <summary>
    /// 序列化帮助类
    /// </summary>
    public class SerializeHelper
    {
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static byte[] Serialize(object item)
        {
            var jsonString = JsonConvert.SerializeObject(item);

            return Encoding.UTF8.GetBytes(jsonString);
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static TEntity Deserialize<TEntity>(byte[] value)
        {
            if (value == null)
            {
                return default(TEntity);
            }
            var jsonString = Encoding.UTF8.GetString(value);
            return JsonConvert.DeserializeObject<TEntity>(jsonString);
        }

        /// <summary>
        /// 读取文件 并反序列化为响应对象
        /// </summary>
        /// <param name="path"></param>
        /// <param name="objectType"></param>
        /// <returns></returns>
        public static object ConvertFileToObject(string path, Type objectType)
        {
            object result = null;
            if (path != null && path.Length > 0)
            {
                using (FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    result = new XmlSerializer(objectType).Deserialize(fileStream);
                    fileStream.Close();
                    return result;
                }
            }
            return result;
        }
    }
}