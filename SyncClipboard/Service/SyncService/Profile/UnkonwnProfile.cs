﻿using System;
using System.Windows.Forms;
using SyncClipboard.Utility;
using static SyncClipboard.Service.ProfileType;

namespace SyncClipboard.Service
{
    class UnkonwnProfile : Profile
    {
        public UnkonwnProfile() { }

        public override string ToolTip()
        {
            throw new NotImplementedException("Do not support this type of clipboard");
        }

        public override void UploadProfile(IWebDav webdav)
        {
            throw new NotImplementedException("Do not support this type of clipboard");
        }

        public override ClipboardType GetProfileType()
        {
            return ClipboardType.Unknown;
        }

        protected override DataObject CreateDataObject()
        {
            throw new NotImplementedException("Do not support this type of clipboard");
        }
    }
}