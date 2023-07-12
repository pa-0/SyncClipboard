﻿using SyncClipboard.Core.Clipboard;
using SyncClipboard.Core.Commons;
using SyncClipboard.Core.Interfaces;
using SyncClipboard.Core.Models;
using SyncClipboard.Core.Utilities.Image;
using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
#nullable enable

namespace SyncClipboard.Service;

public abstract class ClipboardFactoryBase : IClipboardFactory
{
    protected abstract ILogger Logger { get; set; }
    protected abstract UserConfig UserConfig { get; set; }
    protected abstract IServiceProvider ServiceProvider { get; set; }
    protected abstract IWebDav WebDav { get; set; }

    public abstract MetaInfomation GetMetaInfomation();

    public Profile CreateProfile(MetaInfomation? metaInfomation = null)
    {
        metaInfomation ??= GetMetaInfomation();

        if (metaInfomation.Files != null)
        {
            var filename = metaInfomation.Files[0];
            if (System.IO.File.Exists(filename))
            {
                if (ImageHelper.FileIsImage(filename))
                {
                    return new ImageProfile(filename, ServiceProvider);
                }
                return new FileProfile(filename, ServiceProvider);
            }
        }

        if (metaInfomation.Text != null)
        {
            return new TextProfile(metaInfomation.Text, ServiceProvider);
        }

        if (metaInfomation.Image != null)
        {
            return ImageProfile.CreateFromImage(metaInfomation.Image, ServiceProvider);
        }

        return new UnkonwnProfile();
    }

    public async Task<Profile> CreateProfileFromRemote(CancellationToken cancelToken)
    {
        ClipboardProfileDTO? profileDTO;
        try
        {
            profileDTO = await WebDav.GetJson<ClipboardProfileDTO>(SyncService.REMOTE_RECORD_FILE, cancelToken);
        }
        catch (Exception ex)
        {
            if (ex is HttpRequestException { StatusCode: HttpStatusCode.NotFound })
            {
                var blankProfile = new TextProfile("", ServiceProvider);
                await blankProfile.UploadProfile(WebDav, cancelToken);
                return blankProfile;
            }
            Logger.Write("CreateFromRemote failed");
            throw;
        }

        ArgumentNullException.ThrowIfNull(profileDTO);
        ProfileType type = ProfileTypeHelper.StringToProfileType(profileDTO.Type);
        return GetProfileBy(type, profileDTO);
    }

    private Profile GetProfileBy(ProfileType type, ClipboardProfileDTO profileDTO)
    {
        switch (type)
        {
            case ProfileType.Text:
                return new TextProfile(profileDTO.Clipboard, ServiceProvider);
            case ProfileType.File:
                {
                    if (ImageHelper.FileIsImage(profileDTO.File))
                    {
                        return new ImageProfile(profileDTO, ServiceProvider);
                    }
                    return new FileProfile(profileDTO, ServiceProvider);
                }
            case ProfileType.Image:
                return new ImageProfile(profileDTO, ServiceProvider);
        }

        return new UnkonwnProfile();
    }
}