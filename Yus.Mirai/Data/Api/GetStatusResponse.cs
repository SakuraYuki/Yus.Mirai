using Newtonsoft.Json;

namespace Yus.Mirai.Data.Api
{
    /*
    {
        "data": {
            "app_enabled": true,
            "app_good": true,
            "app_initialized": true,
            "good": true,
            "online": true,
            "plugins_good": null,
            "stat": {
                "packet_received": 149,
                "packet_sent": 138,
                "packet_lost": 0,
                "message_received": 5,
                "message_sent": 0,
                "disconnect_times": 0,
                "lost_times": 0,
                "last_message_time": 1631350423
            }
        },
        "retcode": 0,
        "status": "ok"
    }
     */

    public class GetStatusDataStat : BaseObservable
    {
        [JsonProperty("packet_received")]
        public int PacketReceived { get; set; }

        [JsonProperty("packet_sent")]
        public int PacketSent { get; set; }

        [JsonProperty("packet_lost")]
        public int PacketLost { get; set; }

        [JsonProperty("message_received")]
        public int MessageReceived { get; set; }

        [JsonProperty("message_sent")]
        public int MessageSent { get; set; }

        [JsonProperty("disconnect_times")]
        public int DisconnectTimes { get; set; }

        [JsonProperty("lost_times")]
        public int LostTimes { get; set; }

        [JsonProperty("last_message_time")]
        public int LastMessageTime { get; set; }
    }

    public class GetStatusData : BaseObservable
    {
        [JsonProperty("app_enabled")]
        public bool AppEnabled { get; set; }

        [JsonProperty("app_good")]
        public bool AppGood { get; set; }

        [JsonProperty("app_initialized")]
        public bool AppInitialized { get; set; }

        [JsonProperty("good")]
        public bool Good { get; set; }

        [JsonProperty("online")]
        public bool Online { get; set; }

        [JsonProperty("plugins_good")]
        public object PluginsGood { get; set; }

        [JsonProperty("stat")]
        public GetStatusDataStat Stat { get; set; }
    }

    public class GetStatusResponse : MiraiApiRespone<GetStatusData>
    {
    }
}
