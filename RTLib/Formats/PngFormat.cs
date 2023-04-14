using System.Drawing.Imaging;

namespace RTLib.Formats;

public class PngFormat : BitmapFormat
{
    protected override ImageFormat Format => ImageFormat.Png;
}