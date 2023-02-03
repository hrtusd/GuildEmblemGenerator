using SkiaSharp;

namespace GuildEmblemGenerator.Core;

public class Generator
{
    public SKImage? EmblemImage { get; private set; }

    public void Generate(int iconId, int borderId, EFaction faction, SKColor backgroundColor, SKColor borderColor, SKColor iconColor)
    {
        var imageInfo = new SKImageInfo(250, 250);

        var surface = SKSurface.Create(imageInfo);

        var canvas = surface.Canvas;

        canvas.Clear();

        var retriever = new Retriever();

        var icon = retriever.ReadIcon(iconId);
        var hooks = retriever.ReadHooks();
        var border = retriever.ReadBorder(borderId);
        var flag = retriever.ReadFlag();
        var background = retriever.ReadBackground(faction);

        using var paint = new SKPaint();

        canvas.DrawImage(background, retriever.ReadPoint(EImageType.Background));

        canvas.DrawImage(flag, retriever.ReadPoint(EImageType.Flag));

        paint.ColorFilter = SKColorFilter.CreateBlendMode(backgroundColor, SKBlendMode.SrcATop);
        paint.BlendMode = SKBlendMode.ColorBurn;

        canvas.DrawImage(flag, retriever.ReadPoint(EImageType.Flag), paint);

        canvas.DrawImage(hooks, retriever.ReadPoint(EImageType.Hooks));

        canvas.DrawImage(border, retriever.ReadPoint(EImageType.Border));
        paint.ColorFilter = SKColorFilter.CreateBlendMode(borderColor, SKBlendMode.SrcATop);
        paint.BlendMode = SKBlendMode.ColorDodge;

        canvas.DrawImage(border, retriever.ReadPoint(EImageType.Border), paint);

        canvas.DrawImage(icon, retriever.ReadPoint(EImageType.Icon));
        paint.ColorFilter = SKColorFilter.CreateBlendMode(iconColor, SKBlendMode.SrcATop);
        paint.BlendMode = SKBlendMode.ColorBurn;

        canvas.DrawImage(icon, retriever.ReadPoint(EImageType.Icon), paint);

        EmblemImage = surface.Snapshot();
    }

    public void SaveToFile(string path)
    {
        if (EmblemImage is null)
        {
            throw new NullReferenceException(nameof(EmblemImage));
        }

        using var data = EmblemImage.Encode(SKEncodedImageFormat.Png, 90);
        using var stream = File.OpenWrite(Path.Combine(path));

        data.SaveTo(stream);
    }

    public Stream GetImageStream()
    {
        if (EmblemImage is null)
        {
            throw new NullReferenceException(nameof(EmblemImage));
        }

        var data = EmblemImage.Encode(SKEncodedImageFormat.Png, 90);

        return data.AsStream();
    }
}