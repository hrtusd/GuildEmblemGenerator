using SkiaSharp;

namespace GuildEmblemGenerator;

public enum EFaction
{
    Alliance, Horde
}

public enum EImageType
{
    Flag, Background, Hooks, Border, Icon
}

public class Retriever
{
    private readonly string baseDir;
    private const string emblemStringFormat = "Images.Icons.emblem_{0}.png";
    private const string borderStringFormat = "Images.Borders.border_{0}.png";

    private readonly Dictionary<EImageType, SKPoint> imagePoints = new();

    public Retriever()
    {
        imagePoints.Add(EImageType.Flag, new SKPoint(37, 47));
        imagePoints.Add(EImageType.Background, new SKPoint(17, 17));
        imagePoints.Add(EImageType.Hooks, new SKPoint(37, 49));
        imagePoints.Add(EImageType.Border, new SKPoint(50, 60));
        imagePoints.Add(EImageType.Icon, new SKPoint(55, 75));

        baseDir = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name!;
    }

    public SKImage ReadIcon(int iconId)
    {
        var iconName = string.Format(emblemStringFormat, iconId.ToString().PadLeft(2, '0'));

        return ReadImage($"{baseDir}.{iconName}");
    }

    public SKImage ReadBackground(EFaction faction)
    {
        var path = faction switch
        {
            EFaction.Alliance => $"{baseDir}.Images.ring-alliance.png",
            EFaction.Horde => $"{baseDir}.Images.ring-horde.png",
            _ => throw new Exception("What are you even doing?"),
        };

        return ReadImage(path);
    }

    public SKImage ReadFlag()
    {
        return ReadImage($"{baseDir}.Images.bg_00.png");
    }

    public SKImage ReadBorder(int borderId)
    {
        var borderName = string.Format(borderStringFormat, borderId.ToString().PadLeft(2, '0'));

        return ReadImage($"{baseDir}.{borderName}");
    }

    public SKImage ReadHooks()
    {
        return ReadImage($"{baseDir}.Images.hooks.png");
    }

    public SKPoint ReadPoint(EImageType imageType)
    {
        imagePoints.TryGetValue(imageType, out var point);

        return point;
    }

    private SKImage ReadImage(string filePath)
    {
        var data = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream(filePath);

        return SKImage.FromEncodedData(data);
    }
}
