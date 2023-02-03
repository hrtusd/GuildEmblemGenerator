using GuildEmblemGenerator.Core;
using SkiaSharp;

namespace GuildEmblemGenerator;

internal class Program
{
    static void Main(string[] args)
    {
        var generator = new Generator();

        var backgroundColor = new SKColor(255, 0, 255);
        var borderColor = new SKColor(0, 255, 255);
        var iconColor = new SKColor(87, 32, 222);

        generator.Generate(0, 0, EFaction.Alliance, backgroundColor, borderColor, iconColor);
    }
}