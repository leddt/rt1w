namespace RTLib.Formats;

public static class PpmFormat
{
    public static void WriteFile(Stream target, Vec3[,] rgbPixels)
    {
        using var writer = new StreamWriter(target);

        var imageWidth = rgbPixels.GetLength(0);
        var imageHeight = rgbPixels.GetLength(1);
        
        writer.WriteLine($"P3\n{imageWidth} {imageHeight}\n255");
        for (var j = imageHeight - 1; j >= 0; j--)
        for (var i = 0; i < imageWidth; i++)
        {
            var px = rgbPixels[i, j];
            writer.WriteLine($"{(int)(256 * Math.Clamp(px.X, 0, 0.999))} " +
                             $"{(int)(256 * Math.Clamp(px.Y, 0, 0.999))} " +
                             $"{(int)(256 * Math.Clamp(px.Z, 0, 0.999))}");
        }
    }
}