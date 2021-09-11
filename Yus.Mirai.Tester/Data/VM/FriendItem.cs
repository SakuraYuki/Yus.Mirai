using Yus.Mirai.Tester.Data.UI;

namespace Yus.Mirai.Tester.Data.VM
{
    public class FriendItem : BaseVM
    {
        private string nickname;
        public string Nickname { get => nickname; set => Set(ref nickname, value, nameof(Nickname)); }

        private string remark;
        public string Remark { get => remark; set => Set(ref remark, value, nameof(Remark)); }

        private long userId;
        public long UserId { get => userId; set => Set(ref userId, value, nameof(userId)); }

        public string ShowLabel => Nickname + (string.IsNullOrWhiteSpace(Remark) ? "" : $"({Remark})");

        public FriendItem()
        {
            PropertyChanged += FriendItem_PropertyChanged;
        }

        private void FriendItem_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e?.PropertyName)
            {
                case nameof(Nickname):
                case nameof(Remark):
                    OnPropertyChanged(nameof(ShowLabel));
                    break;
                default:
                    break;
            }
        }
    }
}
