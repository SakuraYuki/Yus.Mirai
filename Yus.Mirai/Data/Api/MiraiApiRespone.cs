using Newtonsoft.Json;

namespace Yus.Mirai.Data.Api
{
    public class MiraiApiRespone : BaseObservable
    {
        [JsonProperty("retcode")]
        public int Retcode { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("msg")]
        public string Msg { get; set; }

        [JsonProperty("wording")]
        public string Wording { get; set; }

        [JsonIgnore]
        public bool IsSuccess => Retcode == 0 && Status == "ok";
    }

    public class MiraiApiRespone<T> : MiraiApiRespone where T : class, new()
    {
        [JsonProperty("data")]
        public T Data { get; set; }
    }
}
