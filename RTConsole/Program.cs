using RTConsole;

// Image
const double aspectRatio = 16.0 / 9.0;
const int imageWidth = 400;
const int imageHeight = (int)(imageWidth / aspectRatio);
const int samplesPerPixel = 100;

// World
var world = new HittableList
{
    new Sphere(new Vec3(0, 0, -1), 0.5),
    new Sphere(new Vec3(0, -100.5, -1), 100)
};

// Camera
var camera = new Camera();

// Render

Console.Write($"P3\n{imageWidth} {imageHeight}\n255\n");

for (var j = imageHeight - 1; j >= 0; j--)
{
    Console.Error.Write($"\rScanlines remaining: {j} ");
    for (var i = 0; i < imageWidth; i++)
    {
        var pixelColor = new Vec3(0, 0, 0);
        
        for (int s = 0; s < samplesPerPixel; s++)
        {
            var u = (i + Random.Shared.NextDouble()) / (imageWidth - 1);
            var v = (j + Random.Shared.NextDouble()) / (imageHeight - 1);
            var r = camera.GetRay(u, v);
            pixelColor += RayColor(r, world);
        }
        
        pixelColor.WriteColor(Console.Out, samplesPerPixel);
    }
}

Console.Error.Write("\nDone.\n");

Vec3 RayColor(Ray r, IHittable world)
{
    var rec = new Hit();
    if (world.Hit(r, 0, double.PositiveInfinity, ref rec))
    {
        var target = rec.P + rec.Normal + Vec3.RandomInUnitSphere();
        return 0.5 * RayColor(new Ray(rec.P, target - rec.P), world);
    }
    
    var unitDirection = Vec3.UnitVector(r.Direction);
    var t = 0.5 * (unitDirection.Y + 1);
    return (1 - t) * new Vec3(1, 1, 1) + t * new Vec3(0.5, 0.7, 1);
}