using Newtonsoft.Json;

namespace RevitCmd
{
    public static class JsonConvertExt
    {
        public static string SerializeJson<T>(this T obj)
            => JsonConvert.SerializeObject(obj);
        public static T DeserializeJson<T>(this string json)
            => (T)JsonConvert.DeserializeObject(json, typeof(T));
    }
}
