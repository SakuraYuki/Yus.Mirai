using Newtonsoft.Json;
using System.Collections.Generic;

namespace Yus.Mirai.Data.Api
{
    /*
    {
        "data": [
            {
                "nickname": "JackMa",
                "remark": "马云",
                "user_id": 16881688
            },
            {
                "nickname": "强子",
                "remark": "刘强东",
                "user_id": 618618
            },
            {
                "nickname": "小龙",
                "remark": "张小龙",
                "user_id": 20112011
            }
        ],
        "retcode": 0,
        "status": "ok"
    }
     */

    public class GetFriendListItem
    {
        [JsonProperty("nickname")]
        public string Nickname { get; set; }

        [JsonProperty("remark")]
        public string Remark { get; set; }

        [JsonProperty("user_id")]
        public long UserId { get; set; }
    }

    public class GetFriendListResponse : MiraiApiRespone<List<GetFriendListItem>>
    {
    }
}
