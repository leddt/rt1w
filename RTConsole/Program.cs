const int imageWidth = 256;
const int imageHeight = 256;

Console.Write($"P3\n{imageWidth} {imageHeight}\n255\n");

for (var j = imageHeight - 1; j >= 0; j--)
{
    Console.Error.Write($"\rScanlines remaining: {j} ");
    for (var i = 0; i < imageWidth; i++)
    {
        var r = (double)i / (imageWidth - 1);
        var g = (double)j / (imageHeight - 1);
        var b = .25;

        var ir = (int)(255.999 * r);
        var ig = (int)(255.999 * g);
        var ib = (int)(255.999 * b);

        Console.Write($"{ir} {ig} {ib}\n");
    }
}

Console.Error.Write("\nDone.\n");