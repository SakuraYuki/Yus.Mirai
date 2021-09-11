using Newtonsoft.Json;

namespace Yus.Mirai.Data.Api
{
    /*
    {
        "data": {
            "level": 1
        },
        "retcode": 0,
        "status": "ok"
    }
     */

    public class CheckUrlSafelyData
    {
        [JsonProperty("level")]
        public int Level { get; set; }
    }

    public class CheckUrlSafelyResponse : MiraiApiRespone<CheckUrlSafelyData>
    {
    }
}
