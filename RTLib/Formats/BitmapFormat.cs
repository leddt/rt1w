using System.Drawing;
using System.Drawing.Imaging;

namespace RTLib.Formats;

public abstract class BitmapFormat : IFormat
{
    protected abstract ImageFormat Format { get; }
    
    public void WriteFile(Stream target, Vec3[,] rgbPixels)
    {
        var imageWidth = rgbPixels.GetLength(0);
        var imageHeight = rgbPixels.GetLength(1);

        using var image = new Bitmap(imageWidth, imageHeight);
        
        for (var x = 0; x < imageWidth; x++)
        for (var y = 0; y < imageHeight; y++)
        {
            var px = rgbPixels[x, y];
            image.SetPixel(x, imageHeight - y - 1, Color.FromArgb(
                (int)(256 * Math.Clamp(px.X, 0, 0.999)),
                (int)(256 * Math.Clamp(px.Y, 0, 0.999)),
                (int)(256 * Math.Clamp(px.Z, 0, 0.999))
            ));
        }
        
        image.Save(target, Format);
    }
}