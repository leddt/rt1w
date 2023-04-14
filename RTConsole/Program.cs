using RTConsole;

const int imageWidth = 256;
const int imageHeight = 256;

Console.Write($"P3\n{imageWidth} {imageHeight}\n255\n");

for (var j = imageHeight - 1; j >= 0; j--)
{
    Console.Error.Write($"\rScanlines remaining: {j} ");
    for (var i = 0; i < imageWidth; i++)
    {
        var pixelColor = new Vec3((double)i / (imageWidth - 1), (double)j / (imageHeight - 1), .25);
        pixelColor.WriteColor(Console.Out);
    }
}

Console.Error.Write("\nDone.\n");