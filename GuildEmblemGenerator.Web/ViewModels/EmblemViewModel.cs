using CommunityToolkit.Mvvm.ComponentModel;
using MudBlazor.Utilities;
using SkiaSharp.Views.Blazor;
using SkiaSharp;
using GuildEmblemGenerator.Core;
using Microsoft.JSInterop;
using CommunityToolkit.Mvvm.Input;

namespace GuildEmblemGenerator.Web.ViewModels;

public partial class EmblemViewModel : ObservableObject
{
    private readonly Generator _generator;
    private readonly IJSRuntime _runtime;

    private SKColor parsedBackgroundColor;
    private SKColor parsedBorderColor;
    private SKColor parsedIconColor;

    [ObservableProperty]
    private SKCanvasView? canvasView;

    [ObservableProperty]
    private EFaction faction;
    
    [ObservableProperty]
    private int icon;
    [ObservableProperty]
    private int border;

    [ObservableProperty]
    private MudColor backgroundColor = "#594AE2";

    [ObservableProperty]
    private MudColor borderColor = "#FF4081";

    [ObservableProperty]
    private MudColor iconColor = "#FF4081";

    public bool CanDownload => _generator.EmblemImage is not null;

    public EmblemViewModel(IJSRuntime runtime)
    {
        _generator = new Generator();
        _runtime = runtime;
    }

    [RelayCommand(CanExecute = nameof(CanDownload))]
    private async Task Download()
    {
        var fileStream = _generator.GetImageStream();
        var fileName = "emblem.png";

        using var streamRef = new DotNetStreamReference(fileStream);

        await _runtime.InvokeVoidAsync("downloadFile", fileName, streamRef);
    }

    partial void OnFactionChanged(EFaction value)
    {
        Recreate();
    }

    partial void OnIconChanged(int value)
    {
        Recreate();
    }

    partial void OnBorderChanged(int value)
    {
        Recreate();
    }

    partial void OnBackgroundColorChanged(MudColor value)
    {
        parsedBackgroundColor = new SKColor(value.R, value.G, value.B);
        Recreate();
    }

    partial void OnBorderColorChanged(MudColor value)
    {
        parsedBorderColor = new SKColor(value.R, value.G, value.B);
        Recreate();
    }

    partial void OnIconColorChanged(MudColor value)
    {
        parsedIconColor = new SKColor(value.R, value.G, value.B);
        Recreate();
    }

    private void Recreate()
    {
        _generator.Generate(
            Icon,
            Border,
            Faction,
            parsedBackgroundColor,
            parsedBorderColor,
            parsedIconColor);

        DownloadCommand.NotifyCanExecuteChanged();

        CanvasView?.Invalidate();
    }

    public void PaintSurface(SKPaintSurfaceEventArgs e)
    {
        var canvas = e.Surface.Canvas;

        canvas.Clear();

        if (_generator.EmblemImage is not null)
        {
            canvas.DrawImage(_generator.EmblemImage, new SKPoint(0, 0));
        }
    }
}
