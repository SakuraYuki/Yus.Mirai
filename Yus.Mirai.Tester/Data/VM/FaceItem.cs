using System;
using Yus.Mirai.Tester.Data.UI;

namespace Yus.Mirai.Tester.Data.VM
{
    public class FaceItem : BaseVM
    {
        private string faceId;
        public string FaceId { get => faceId; set => Set(ref faceId, value, nameof(FaceId)); }

        private string name;
        public string Name { get => name; set => Set(ref name, value, nameof(Name)); }

        private Uri resourceUri;
        public Uri ResourceUri { get => resourceUri; set => Set(ref resourceUri, value, nameof(ResourceUri)); }

        private Uri gifResourceUri;
        public Uri GifResourceUri { get => gifResourceUri; set => Set(ref gifResourceUri, value, nameof(GifResourceUri)); }

        public string CQcode => $"[CQ:face,id={FaceId}]";

        public FaceItem()
        {
            PropertyChanged += FaceItem_PropertyChanged;
        }

        public FaceItem(string faceId)
        {
            PropertyChanged += FaceItem_PropertyChanged;
            SetupFaceId(faceId);
        }

        public FaceItem(int faceId) : this(faceId.ToString())
        {

        }

        private void FaceItem_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e?.PropertyName)
            {
                case nameof(FaceId):
                    OnPropertyChanged(nameof(CQcode));
                    break;
                default:
                    break;
            }
        }

        public void SetupFaceId(string faceId)
        {
            FaceId = faceId;
            Name = faceId;
            ResourceUri = new Uri($"/Yus.Mirai.Tester;component/Resources/FacePng/face_{faceId}.gif", UriKind.Relative);
            GifResourceUri = new Uri($"/Yus.Mirai.Tester;component/Resources/FaceGif/face_{faceId}.gif", UriKind.Relative);
        }
    }
}
