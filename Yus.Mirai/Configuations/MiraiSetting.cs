namespace Yus.Mirai.Configuations
{
    public class MiraiSetting
    {
        public string ApiRoot { get; set; }

        public string MiraiServerDir { get; set; }

        public int HeartInterval { get; set; }

        public bool CheckHeart { get; set; }

        public static MiraiSetting Default => new MiraiSetting
        {
            ApiRoot = "http://127.0.0.1:5700",
            HeartInterval = 10000,
            CheckHeart = true,
        };
    }
}
