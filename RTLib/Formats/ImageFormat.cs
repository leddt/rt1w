using SixLabors.ImageSharp.Formats;

namespace RTLib.Formats;

public abstract class ImageFormat : IFormat
{
    protected abstract IImageEncoder GetEncoder();
    
    public void WriteFile(Stream target, Vec3[,] rgbPixels)
    {
        var imageWidth = rgbPixels.GetLength(0);
        var imageHeight = rgbPixels.GetLength(1);

        using var image = new Image<Rgb24>(imageWidth, imageHeight);
        
        for (var x = 0; x < imageWidth; x++)
        for (var y = 0; y < imageHeight; y++)
        {
            var px = rgbPixels[x, y];
            image[x, imageHeight - y - 1] = new Rgb24(
                (byte)(256 * Math.Clamp(px.X, 0, 0.999)),
                (byte)(256 * Math.Clamp(px.Y, 0, 0.999)),
                (byte)(256 * Math.Clamp(px.Z, 0, 0.999))
            );
        }
        
        image.Save(target, GetEncoder());
    }
}