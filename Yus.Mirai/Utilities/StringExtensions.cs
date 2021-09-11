using Newtonsoft.Json;

namespace Yus.Mirai.Utilities
{
    /// <summary>
    /// 字符串相关扩展
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// 将 <see cref="string"/>(JSON) 转换到指定对象
        /// </summary>
        /// <param name="value"><see cref="string"/>(JSON) 值</param>
        /// <param name="settings">反序列化设置</param>
        /// <returns></returns>
        public static T ToObject<T>(this string value, JsonSerializerSettings settings = null)
        {
            return JsonHelper.ToObject<T>(value, settings: settings);
        }
    }
}
