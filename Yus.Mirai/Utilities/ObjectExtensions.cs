using Newtonsoft.Json;

namespace Yus.Mirai.Utilities
{
    public static class ObjectExtensions
    {
        /// <summary>
        /// 转换为 JSON
        /// </summary>
        /// <param name="obj">要序列化的对象</param>
        /// <param name="formatting">JSON格式，对齐还是压缩</param>
        /// <param name="settings">序列化设置</param>
        /// <returns></returns>
        public static string ToJson(this object obj, Formatting formatting = Formatting.None, JsonSerializerSettings settings = null)
        {
            return obj == null ? null : JsonHelper.ToJson(obj, formatting: formatting, settings: settings);
        }
    }
}
