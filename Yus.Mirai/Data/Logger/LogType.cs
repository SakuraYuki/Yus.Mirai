namespace Yus.Mirai.Data.Logger
{
    /// <summary>
    /// 日志类型
    /// </summary>
    [System.Flags]
    public enum LogType
    {
        /// <summary>
        /// 冗余
        /// </summary>
        Verbose = 1,

        /// <summary>
        /// 调试
        /// </summary>
        Debug = 2,

        /// <summary>
        /// 信息
        /// </summary>
        Info = 4,

        /// <summary>
        /// 警告
        /// </summary>
        Warn = 8,

        /// <summary>
        /// 错误
        /// </summary>
        Error = 16,

        /// <summary>
        /// 致命
        /// </summary>
        Fatal = 32
    }
}
