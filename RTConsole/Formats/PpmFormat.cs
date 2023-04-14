namespace RTConsole.Formats;

public static class PpmFormat
{
    public static void WriteFile(Stream target, Vec3[,] canvas, int samplesPerPixel)
    {
        using var writer = new StreamWriter(target);

        var imageWidth = canvas.GetLength(0);
        var imageHeight = canvas.GetLength(1);
        
        writer.WriteLine($"P3\n{imageWidth} {imageHeight}\n255");
        for (var j = imageHeight - 1; j >= 0; j--)
        for (var i = 0; i < imageWidth; i++)
            canvas[i, j].WriteColor(writer, samplesPerPixel);
    }
}