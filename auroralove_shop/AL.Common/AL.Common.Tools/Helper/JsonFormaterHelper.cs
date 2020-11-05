using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace AL.Common.Tools.Helper
{
    public static class JsonFormaterHelper
    {
        public static T Deserialize<T>(string jsonDatas, Encoding encoding)
        {
            T result = default(T);
            if (string.IsNullOrEmpty(jsonDatas))
            {
                return result;
            }
            DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(T));
            byte[] bytes = encoding.GetBytes(jsonDatas);
            using (MemoryStream memoryStream = new MemoryStream(bytes))
            {
                memoryStream.Position = 0L;
                return (T)dataContractJsonSerializer.ReadObject(memoryStream);
            }
        }

        public static string Serialize<T>(T obj)
        {
            if (obj == null)
            {
                return null;
            }
            DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(T));
            using (MemoryStream memoryStream = new MemoryStream())
            {
                dataContractJsonSerializer.WriteObject(memoryStream, obj);
                memoryStream.Position = 0L;
                using (StreamReader streamReader = new StreamReader(memoryStream))
                {
                    return streamReader.ReadToEnd();
                }
            }
        }

        public static string JsonToString(object JsonData)
        {
            return JsonConvert.SerializeObject(JsonData);
        }

        public static string JsonToString(object JsonData, params JsonConverter[] converters)
        {
            return JsonConvert.SerializeObject(JsonData, converters);
        }

        public static string JsonToString(object JsonData, JsonSerializerSettings settings)
        {
            return JsonConvert.SerializeObject(JsonData, settings);
        }

        public static string JsonToString(object JsonData, string DateFormat, string JsonCallBack)
        {
            //IL_0039: Unknown result type (might be due to invalid IL or missing references)
            //IL_0040: Expected O, but got Unknown
            if (string.IsNullOrEmpty(JsonCallBack) && string.IsNullOrEmpty(DateFormat))
            {
                return JsonToString(JsonData);
            }
            string empty = string.Empty;
            if (string.IsNullOrEmpty(DateFormat))
            {
                empty = JsonToString(JsonData);
            }
            else
            {
                IsoDateTimeConverter val = new IsoDateTimeConverter();
                val.DateTimeFormat = DateFormat;
                empty = JsonToString(JsonData, (JsonConverter[])new JsonConverter[1]
                {
                    val
                });
            }
            if (string.IsNullOrEmpty(JsonCallBack))
            {
                return empty;
            }
            return JsonCallBack + "(" + empty + ")";
        }

        public static T StringToObject<T>(string strJson)
        {
            return JsonConvert.DeserializeObject<T>(strJson);
        }

        public static T StringToObject<T>(string strJson, JsonSerializerSettings settings)
        {
            return JsonConvert.DeserializeObject<T>(strJson, settings);
        }

        public static string JsonToString(object JsonData, string JsonCallBack)
        {
            return JsonToString(JsonData, string.Empty, JsonCallBack);
        }
    }
}