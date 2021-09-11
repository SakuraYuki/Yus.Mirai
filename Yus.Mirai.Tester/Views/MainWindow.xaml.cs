using System;
using System.Collections.ObjectModel;
using Yus.Mirai.Tester.Apps;
using Yus.Mirai.Tester.Data.VM;
using Yus.Mirai.Utilities;

namespace Yus.Mirai.Tester.Views
{
    public partial class MainWindow : BaseWindow
    {
        #region Properties

        /// <summary>
        /// 对应VM
        /// </summary>
        public MainViewModel VM { get; set; }

        /// <summary>
        /// 日志内容
        /// </summary>
        public string LogText { get => TxtLog.Text; set => TxtLog.Text = value; }

        public override System.Windows.Controls.Panel GrowlPanel => PanelGrowl;

        #endregion Properties

        #region Ctor

        /// <summary>
        /// 默认构造
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            DataContext = VM = new MainViewModel(this);
            Loaded += MainWindow_Loaded;
        }

        #endregion Ctor

        #region Event Handlers

        private void MainWindow_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            LogText = $"欢迎使用 「Yus.Mirai测试器」";

            TxtLog.TextChanged += TxtLog_TextChanged;
            BtnScrollLog.Checked += (s, ee) => BtnScrollLog.ToolTip = "日志滚动";
            BtnScrollLog.Unchecked += (s, ee) => BtnScrollLog.ToolTip = "日志固定";
        }

        private void TxtLog_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (TxtLog.Text.Length > 20000)
            {
                TxtLog.Clear();
            }

            if (BtnScrollLog.IsChecked.Value)
            {
                TxtLog.ScrollToEnd();
            }
        }

        #endregion Event Handlers

        #region Logic Methods

        #region Logger

        /// <summary>
        /// 追加日志
        /// </summary>
        /// <param name="log">日志内容</param>
        public void AppendLog(string log)
        {
            if (!string.IsNullOrWhiteSpace(TxtLog.Text))
            {
                TxtLog.AppendText(Environment.NewLine);
            }
            TxtLog.AppendText(log);
        }

        #endregion Logger

        #endregion Logic Methods
    }

    public class MainViewModel : BaseViewModel
    {
        #region Properties

        /// <summary>对应视图</summary>
        public MainWindow View { get; set; }

        /// <summary>
        /// 服务API
        /// </summary>
        public MiraiApi Api { get; set; }

        #endregion Properties

        #region Building Properties

        private bool isServerConnect;
        /// <summary>
        /// 服务是否连接
        /// </summary>
        public bool IsServerConnect { get => isServerConnect; set => Set(ref isServerConnect, value, nameof(IsServerConnect)); }

        private ObservableCollection<FriendItem> friends;
        /// <summary>
        /// 好友列表
        /// </summary>
        public ObservableCollection<FriendItem> Friends { get => friends; set => Set(ref friends, value, nameof(friends)); }

        #endregion Building Properties

        #region Ctor

        /// <summary>
        /// 默认构造
        /// </summary>
        /// <param name="view">对应窗体</param>
        public MainViewModel(MainWindow view)
        {
            View = view;
            Friends = new ObservableCollection<FriendItem>();

            Messager.NewLog += Messager_NewLog;

            Api = new MiraiApi();
            Api.NewLog += Api_NewLog;
            Api.Heart += Api_Heart;
        }

        #endregion Ctor

        #region Event Handlers

        private void OnPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e == null || e.PropertyName == null) return;

            switch (e.PropertyName)
            {
                default: break;
            }
        }

        private void Messager_NewLog(object sender, LogEventArgs e)
        {
            if (!e.IsShow)
            {
                return;
            }

            UiLog(e.Log, e.Exception);
        }

        private void Api_NewLog(object sender, Mirai.Data.Logger.LogEventArgs e)
        {
            var tag = string.IsNullOrWhiteSpace(e.Tag) ? "" : $"[{e.Tag}]";
            Log($"[API]{tag}{e.Log}", (LogType)(int)e.Type, e.Exception);
        }

        private void Api_Heart(object sender, bool e)
        {
            if (e)
            {
                var onlineStr = Api.IsOnline ? "在线" : "离线";
                Log($"[API][心跳]状态：{onlineStr}，{DateTime.Now:G}");
                IsServerConnect = true;
            }
            else
            {
                Log($"[API][心跳]服务已失去连接，{DateTime.Now:G}");
                IsServerConnect = false;
            }
        }

        #endregion Event Handlers

        #region Command Methods

        /// <summary>
        /// 执行命令分发
        /// </summary>
        /// <param name="args">命令标签</param>
        public override void Execute(string args)
        {
            base.Execute(args);
            switch (args)
            {
                // 文件
                case "清空日志": ClearLog(); break;
                case "退出软件": ExitApp(); break;

                // 服务
                case "连接服务": ConnectServer(); break;
                case "断开服务": DisconnectServer(); break;
                case "获取状态": GetStatus(); break;

                // 好友
                case "获取好友列表": GetFriendList(); break;

                default: break;
            }
        }

        /// <summary>
        /// 清空日志
        /// </summary>
        public void ClearLog()
        {
            View.LogText = string.Empty;
            Messager.Notify("日志已清空", NotifyType.Success);
        }

        /// <summary>
        /// 退出软件
        /// </summary>
        /// <param name="needConfirm">是否需要确认</param>
        public bool ExitApp(bool needConfirm = true)
        {
            if (!needConfirm)
            {
                Environment.Exit(0);
                return true;
            }

            var result = ShowMessageBox($"确定要退出Yus.Mirai测试器吗？", button: System.Windows.MessageBoxButton.YesNo, owner: View);
            if (result == System.Windows.MessageBoxResult.Yes)
            {
                Environment.Exit(0);
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 连接服务
        /// </summary>
        public async void ConnectServer()
        {
            if (IsServerConnect)
            {
                return;
            }

            Api.StartHeartCheck();
            var _ = await Api.GetStatus();

            if (Api.IsAvailable)
            {
                Log("服务连接成功");
                IsServerConnect = true;
            }
            else
            {
                Log("服务连接失败");
                IsServerConnect = false;
            }
        }

        /// <summary>
        /// 断开服务
        /// </summary>
        public void DisconnectServer()
        {
            if (!IsServerConnect)
            {
                return;
            }

            Api.StopHeartCheck();

            Log("服务断开成功");
            IsServerConnect = false;
        }

        /// <summary>
        /// 获取状态
        /// </summary>
        public async void GetStatus()
        {
            if (!IsServerConnect)
            {
                Log("服务不可用，请先连接服务");
                return;
            }

            var resp = await Api.GetStatus();
            if (resp == null || !resp.IsSuccess)
            {
                Log("获取状态失败");
                return;
            }

            Log(resp.Data.ToJson(Newtonsoft.Json.Formatting.Indented));
        }

        public async void GetFriendList()
        {
            if (!IsServerConnect)
            {
                Log("服务不可用，请先连接服务");
                return;
            }

            var resp = await Api.GetFriendList();
            if (resp == null || !resp.IsSuccess)
            {
                Log("获取好友列表失败");
                return;
            }

            var friendList = resp.Data;
            Friends.Clear();
            foreach (var item in friendList)
            {
                Friends.Add(new FriendItem()
                {
                    Nickname = item.Nickname,
                    Remark = item.Remark,
                    UserId = item.UserId
                });
            }

            Log(resp.Data.ToJson(Newtonsoft.Json.Formatting.Indented));
        }

        #endregion Command Methods

        #region Logic Methods

        #region Logger

        /// <summary>
        /// 写入日志
        /// </summary>
        /// <param name="log">日志内容</param>
        /// <param name="type">日志类型</param>
        /// <param name="ex">异常对象</param>
        /// <param name="show">是否显示在日志面板</param>
        public void Log(string log, LogType type = LogType.Info, Exception ex = null, bool show = true)
        {
            Messager.Log(log, logType: type, ex: ex, isShow: false);

            if (show)
            {
                UiLog(log, ex: ex);
            }
        }

        /// <summary>
        /// 写入日志到界面
        /// </summary>
        /// <param name="log">日志内容</param>
        /// <param name="type">日志类型</param>
        /// <param name="ex">异常对象</param>
        public void UiLog(string log, Exception ex = null)
        {
            RunUi(() =>
            {
                View.AppendLog(log);
                if (ex != null)
                {
                    View.AppendLog(ExceptionFormat(ex));
                }
            });
        }

        /// <summary>
        /// 格式化异常信息
        /// </summary>
        /// <param name="ex">异常对象</param>
        /// <returns></returns>
        public string ExceptionFormat(Exception ex)
        {
            return $"[错误类型]{ex.GetType().FullName}" +
                $"[错误消息]{ex.Message}" +
                $"{(ex.StackTrace == null ? "" : $"[堆栈信息]{ex.StackTrace}")}";
        }

        #endregion Logger

        #endregion Logic Methods
    }
}
