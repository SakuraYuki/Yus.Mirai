using System;

namespace Yus.Mirai.Data.Logger
{
    public class LogEventArgs : EventArgs
    {
        public LogType Type { get; set; }

        public string Log { get; set; }

        public Exception Exception { get; set; }

        public string Tag { get; set; }

        public DateTime Time { get; set; }

        public LogEventArgs(string log, LogType type = LogType.Info, Exception ex = null, string tag = null, DateTime? time = null)
        {
            Log = log;
            Type = type;
            Exception = ex;
            Tag = tag;
            Time = time ?? DateTime.Now;
        }
    }
}
