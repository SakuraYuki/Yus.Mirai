using RestSharp;
using System;
using System.Threading.Tasks;
using System.Timers;
using Yus.Mirai.Configuations;
using Yus.Mirai.Data.Api;
using Yus.Mirai.Data.Logger;

namespace Yus.Mirai
{
    public class MiraiApi : Data.BaseObservable
    {
        #region Events

        public event EventHandler<bool> Heart;

        public event EventHandler<LogEventArgs> NewLog;

        #endregion Events

        #region Properties

        public MiraiSetting Setting { get; private set; }

        public IRestClient Client { get; private set; }

        public Timer HeartTimer { get; private set; }

        private bool isHeartRunning;
        public bool IsHeartRunning { get => isHeartRunning; private set => Set(ref isHeartRunning, value, nameof(IsHeartRunning)); }

        private DateTime? lastHeartTime;
        public DateTime? LastHeartTime { get => lastHeartTime; private set => Set(ref lastHeartTime, value, nameof(LastHeartTime)); }

        public string LastHeartTimeString => LastHeartTime.HasValue ? LastHeartTime.Value.ToString("yyyy-MM-dd HH:mm:ss") : "-";

        private GetStatusData status;
        public GetStatusData Status { get => status; private set => Set(ref status, value, nameof(Status)); }

        public bool IsAvailable => Status != null;

        public bool IsOnline => Status != null && Status.Online;

        #endregion Properties

        #region Ctor

        public MiraiApi()
        {
            Setting = MiraiSetting.Default;
            InitApi();
        }

        public MiraiApi(MiraiSetting setting)
        {
            Setting = setting;
            InitApi();
        }

        public MiraiApi(string apiRoot)
        {
            Setting = MiraiSetting.Default;
            Setting.ApiRoot = apiRoot;
            InitApi();
        }

        #endregion Ctor

        #region Event Handlers

        private void MiraiApi_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(e?.PropertyName))
            {
                return;
            }
            switch (e.PropertyName)
            {
                case nameof(Status):
                    OnPropertyChanged(nameof(IsAvailable));
                    OnPropertyChanged(nameof(IsOnline));
                    break;
                case nameof(LastHeartTime):
                    OnPropertyChanged(nameof(LastHeartTimeString));
                    break;
                default:
                    break;
            }
        }

        private async void HeartTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            var statusResp = await GetStatus();

            if (statusResp == null || !statusResp.IsSuccess)
            {
                Status = null;
                Heart?.Invoke(this, false);
                RunHeartTimer();
                return;
            }

            Status = statusResp.Data;
            LastHeartTime = DateTime.Now;

            Heart?.Invoke(this, Status.Online);
            RunHeartTimer();
        }

        #endregion Override Methods

        #region Logic Methods

        #region Init

        private void InitApi()
        {
            if (Setting.CheckHeart)
            {
                HeartTimer = new Timer(Setting.HeartInterval)
                {
                    AutoReset = false,
                    Enabled = false
                };
                HeartTimer.Elapsed += HeartTimer_Elapsed;
            }

            Client = new RestClient(Setting.ApiRoot)
            {
                Timeout = 60000,
                ReadWriteTimeout = 60000,
            };

            PropertyChanged += MiraiApi_PropertyChanged;
        }

        #endregion Init

        #region Http

        private async Task<IRestResponse<T>> GetResponse<T>(IRestRequest req, string tag = null) where T : MiraiApiRespone
        {
            try
            {
                var resp = await Client.ExecuteAsync<T>(req);
                Log("请求成功", type: LogType.Debug, tag: tag);
                return resp;
            }
            catch (Exception ex)
            {
                Log($"请求错误，{ex.Message}", type: LogType.Error, ex: ex, tag: tag);
                return null;
            }
        }

        private T CheckResponse<T>(IRestResponse<T> resp, string tag = null) where T : MiraiApiRespone
        {
            if (!resp.IsSuccessful)
            {
                Log($"请求失败，{resp.StatusDescription}({resp.StatusCode})", tag: tag);
                return default;
            }

            if (resp.Data == null)
            {
                Log($"请求数据为空", tag: tag);
                return default;
            }

            var data = resp.Data;

            if (data.Retcode != 0)
            {
                Log($"请求失败，{data.Status}({data.Retcode})", tag: tag);
                return null;
            }

            return data;
        }

        #endregion Http

        #region Heart

        private void RunHeartTimer()
        {
            if (!HeartTimer.Enabled)
            {
                HeartTimer.Enabled = true;
            }
            HeartTimer.Start();
            IsHeartRunning = true;
        }

        #endregion Heart

        #region Logger

        private void Log(string log, LogType type = LogType.Info, Exception ex = null, string tag = null)
        {
            NewLog?.Invoke(this, new LogEventArgs(log, type, ex, tag));
        }

        #endregion Logger

        #endregion Logic Methods

        #region Public Methods

        public bool StartHeartCheck(int? interval = null)
        {
            if (interval.HasValue && interval.Value > 1000)
            {
                Setting.HeartInterval = interval.Value;
            }

            if (!HeartTimer.Enabled)
            {
                Setting.CheckHeart = true;
                RunHeartTimer();
            }

            return true;
        }

        public bool StopHeartCheck()
        {
            if (HeartTimer.Enabled)
            {
                HeartTimer.Stop();
                IsHeartRunning = false;
                HeartTimer.Enabled = false;
                Setting.CheckHeart = false;
            }

            return true;
        }

        public async Task<GetStatusResponse> GetStatus()
        {
            var tag = "获取状态";

            var req = new RestRequest(MiraiApiUrls.GetStatus, Method.POST);

            var resp = await GetResponse<GetStatusResponse>(req, tag: tag);

            var statusResp = CheckResponse(resp, tag: tag);

            Status = statusResp?.Data;

            return statusResp;
        }

        public async Task<CheckUrlSafelyResponse> CheckUrlSafely(string url)
        {
            var tag = "检查连接安全性";

            var req = new RestRequest(MiraiApiUrls.CheckUrlSafely, Method.POST);
            req.AddParameter("url", ParameterType.RequestBody);

            var resp = await GetResponse<CheckUrlSafelyResponse>(req, tag: tag);

            return CheckResponse(resp, tag: tag);
        }

        public async Task<GetFriendListResponse> GetFriendList()
        {
            var tag = "获取好友列表";

            var req = new RestRequest(MiraiApiUrls.GetFriendList, Method.POST);

            var resp = await GetResponse<GetFriendListResponse>(req, tag: tag);

            return CheckResponse(resp, tag: tag);
        }

        #endregion Public Methods
    }
}
