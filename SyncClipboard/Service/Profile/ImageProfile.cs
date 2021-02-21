﻿using System.Drawing;
using static SyncClipboard.Service.ProfileType;

namespace SyncClipboard.Service
{
    class ImageProfile : Profile
    {
        public Image image;
        public ImageProfile(Image image)
        {
            this.image = image;
        }

        protected override ClipboardType GetProfileType()
        {
            return ClipboardType.Image;
        }

        public override void UploadProfile()
        {
            HttpWebResponseUtility.PutImage(Config.GetImageUrl(), this.GetImage(), Config.TimeOut, Config.GetHttpAuthHeader());
            HttpWebResponseUtility.PutText(Config.GetProfileUrl(), this.ToJsonString(), Config.TimeOut, Config.GetHttpAuthHeader());
        }

        protected override void SetContentToLocalClipboard()
        {
            // TODO
        }


        private Image GetImage()
        {
            return image;
        }
    }
}
