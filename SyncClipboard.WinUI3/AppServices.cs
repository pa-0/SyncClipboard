﻿using Microsoft.Extensions.DependencyInjection;
using SyncClipboard.Core;
using SyncClipboard.Core.Clipboard;
using SyncClipboard.Core.Interfaces;
using SyncClipboard.WinUI3.ClipboardWinUI;
using SyncClipboard.WinUI3.Views;

namespace SyncClipboard.WinUI3;

public class AppServices
{
    public static ServiceCollection ConfigureServices()
    {
        var services = new ServiceCollection();

        ProgramWorkflow.ConfigCommonService(services);
        ProgramWorkflow.ConfigurateViewModels(services);

        services.AddSingleton<IMainWindow, SettingWindow>();
        services.AddSingleton<IClipboardChangingListener, ClipboardListener>();
        services.AddSingleton<IClipboardFactory, ClipboardFactory>();
        services.AddSingleton<TrayIcon>();
        services.AddSingleton<ITrayIcon, TrayIconImpl>();
        services.AddSingleton<IContextMenu, TrayIconContextMenu>();

        services.AddTransient<IClipboardSetter<TextProfile>, TextClipboardSetter>();
        services.AddTransient<IClipboardSetter<FileProfile>, FileClipboardSetter>();
        services.AddTransient<IClipboardSetter<ImageProfile>, ImageClipboardSetter>();

        return services;
    }
}
