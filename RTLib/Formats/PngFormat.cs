using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Png;

namespace RTLib.Formats;

public class PngFormat : ImageFormat
{
    protected override IImageEncoder GetEncoder() => new PngEncoder();
}