using HandyControl.Controls;
using HandyControl.Data;
using System;

namespace Yus.Mirai.Tester.Apps
{
    /// <summary>
    /// 消息器
    /// </summary>
    public class Messager
    {
        /// <summary>
        /// 记录新日志时发生
        /// </summary>
        public static event EventHandler<LogEventArgs> NewLog;

        /// <summary>
        /// 出现新通知时发生
        /// </summary>
        public static event EventHandler<NotifyEventArgs> NewNotify;

        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="log">日志内容</param>
        /// <param name="logType">日志等级</param>
        /// <param name="ex">异常信息</param>
        /// <param name="isShow">是否显示在界面上</param>
        public static void Log(string log, LogType logType = LogType.Info, Exception ex = null, bool isShow = false)
        {
            NewLog?.Invoke(null, new LogEventArgs(log, logType: logType, ex: ex, isShow: isShow));
        }

        /// <summary>
        /// 发送通知
        /// </summary>
        /// <param name="notify">通知内容</param>
        /// <param name="notifyType">通知等级</param>
        /// <param name="isGlobal">是否全局通知</param>
        public static void Notify(string notify, NotifyType notifyType = NotifyType.Success, bool isGlobal = false)
        {
            Notify(new NotifyEventArgs(notify, notifyType: notifyType, isGlobal: isGlobal));
        }

        /// <summary>
        /// 发送询问通知
        /// </summary>
        /// <param name="notify">通知内容</param>
        /// <param name="onClose">用户点击按钮（Confirm或者Cancel）时的操作，返回true关闭通知，返回false不关闭通知</param>
        /// <param name="confirmText">确认按钮文字</param>
        /// <param name="cancelText">取消按钮文字</param>
        /// <param name="notifyType">通知等级</param>
        /// <param name="isGlobal">是否全局通知</param>
        public static void NotifyAsk(string notify, Func<bool, bool> onClose, string confirmText = "确认", string cancelText = "关闭", NotifyType notifyType = NotifyType.Success, bool isGlobal = false)
        {
            Notify(new NotifyEventArgs(notify, notifyType: notifyType, isGlobal: isGlobal)
            {
                OnClose = onClose,
                ConfirmText = confirmText,
                CancelText = cancelText,
                IsAsk = true,
            });
        }

        /// <summary>
        /// 发送通知
        /// </summary>
        /// <param name="options">通知选项</param>
        public static void Notify(NotifyEventArgs options)
        {
            var notifyType = InfoType.Success;

            switch (options.NotifyType)
            {
                case NotifyType.Success:
                    notifyType = InfoType.Success;
                    break;
                case NotifyType.Info:
                    notifyType = InfoType.Info;
                    break;
                case NotifyType.Warn:
                    notifyType = InfoType.Warning;
                    break;
                case NotifyType.Error:
                    notifyType = InfoType.Error;
                    break;
                case NotifyType.Fatal:
                    notifyType = InfoType.Fatal;
                    break;
            }

            var info = new GrowlInfo()
            {
                ActionBeforeClose = options.OnClose,
                CancelStr = options.CancelText,
                ConfirmStr = options.ConfirmText,
                Message = options.Notify,
                ShowCloseButton = options.ShowCloseButton,
                ShowDateTime = options.ShowDateTime,
                StaysOpen = options.StaysOpen,
                Type = notifyType,
                WaitTime = options.WaitTime,
            };

            if (options.IsAsk)
            {
                if (options.IsGlobal)
                {
                    Growl.AskGlobal(info);
                }
                else
                {
                    Growl.Ask(info);
                }
            }
            else
            {
                if (options.IsGlobal)
                {
                    switch (options.NotifyType)
                    {
                        case NotifyType.Success:
                            Growl.SuccessGlobal(info);
                            break;
                        case NotifyType.Info:
                            Growl.InfoGlobal(info);
                            break;
                        case NotifyType.Warn:
                            Growl.WarningGlobal(info);
                            break;
                        case NotifyType.Error:
                            Growl.ErrorGlobal(info);
                            break;
                        case NotifyType.Fatal:
                            Growl.FatalGlobal(info);
                            break;
                    }
                }
                else
                {
                    switch (options.NotifyType)
                    {
                        case NotifyType.Success:
                            Growl.Success(info);
                            break;
                        case NotifyType.Info:
                            Growl.Info(info);
                            break;
                        case NotifyType.Warn:
                            Growl.Warning(info);
                            break;
                        case NotifyType.Error:
                            Growl.Error(info);
                            break;
                        case NotifyType.Fatal:
                            Growl.Fatal(info);
                            break;
                    }
                }
            }


            NewNotify?.Invoke(null, options);
        }

        /// <summary>
        /// 清除全部通知（除全局通知）
        /// </summary>
        public static void ClearNotify()
        {
            Growl.Clear();
        }

        /// <summary>
        /// 清除全部全局通知
        /// </summary>
        public static void ClearGlobalNotify()
        {
            Growl.ClearGlobal();
        }

        /// <summary>
        /// 清除全部通知
        /// </summary>
        public static void ClearAllNotify()
        {
            Growl.Clear();
            Growl.ClearGlobal();
        }
    }

    /// <summary>
    /// 日志事件参数
    /// </summary>
    public class LogEventArgs : EventArgs
    {
        /// <summary>
        /// 日志类型
        /// </summary>
        public LogType LogType { get; set; }

        /// <summary>
        /// 日志内容
        /// </summary>
        public string Log { get; set; }

        /// <summary>
        /// 异常内容
        /// </summary>
        public Exception Exception { get; set; }

        /// <summary>
        /// 是否显示在界面上
        /// </summary>
        public bool IsShow { get; set; }

        public LogEventArgs(string log, LogType logType = LogType.Info, Exception ex = null, bool isShow = false)
        {
            Log = log;
            LogType = logType;
            Exception = ex;
            IsShow = isShow;
        }
    }

    /// <summary>
    /// 通知事件参数
    /// </summary>
    public class NotifyEventArgs : EventArgs
    {
        /// <summary>
        /// 通知类型
        /// </summary>
        public NotifyType NotifyType { get; set; }

        /// <summary>
        /// 通知内容
        /// </summary>
        public string Notify { get; set; }

        /// <summary>
        /// 是否全局通知
        /// </summary>
        public bool IsGlobal { get; set; }

        /// <summary>
        /// 是否为询问通知
        /// </summary>
        public bool IsAsk { get; set; }

        /// <summary>
        /// 是否显示通知时间
        /// </summary>
        public bool ShowDateTime { get; set; }

        /// <summary>
        /// 显示时间，单位秒。当<see cref="StaysOpen"/>为true时，此项无效
        /// </summary>
        public int WaitTime { get; set; }

        /// <summary>
        /// 取消按钮文字
        /// </summary>
        public string CancelText { get; set; }

        /// <summary>
        /// 确认按钮文字
        /// </summary>
        public string ConfirmText { get; set; }

        /// <summary>
        /// 用户点击按钮（Confirm或者Cancel）时的操作，返回true关闭通知，返回false不关闭通知
        /// </summary>
        public Func<bool, bool> OnClose { get; set; }

        /// <summary>
        /// 是否持续显示
        /// </summary>
        public bool StaysOpen { get; set; }

        /// <summary>
        /// 是否显示关闭按钮
        /// </summary>
        public bool ShowCloseButton { get; set; }

        /// <summary>
        /// 使用必要通知参数构造
        /// </summary>
        /// <param name="notify">通知内容</param>
        /// <param name="notifyType">通知类型</param>
        /// <param name="isGlobal">是否全局显示</param>
        public NotifyEventArgs(string notify, NotifyType notifyType = NotifyType.Success, bool isGlobal = false)
        {
            Notify = notify;
            NotifyType = notifyType;
            IsGlobal = isGlobal;
            IsAsk = false;
            ShowDateTime = true;
            WaitTime = 6;
            CancelText = "关闭";
            ConfirmText = "确认";
            StaysOpen = false;
            ShowCloseButton = true;
        }
    }

    /// <summary>
    /// 日志类型
    /// </summary>
    [Flags]
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

    /// <summary>
    /// 通知类型
    /// </summary>
    [Flags]
    public enum NotifyType
    {
        /// <summary>
        /// 成功
        /// </summary>
        Success = 1,

        /// <summary>
        /// 信息
        /// </summary>
        Info = 2,

        /// <summary>
        /// 警告
        /// </summary>
        Warn = 4,

        /// <summary>
        /// 错误
        /// </summary>
        Error = 8,

        /// <summary>
        /// 致命
        /// </summary>
        Fatal = 16
    }
}
