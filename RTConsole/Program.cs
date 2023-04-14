using RTConsole;

// Image
const double aspectRatio = 16.0 / 9.0;
const int imageWidth = 400;
const int imageHeight = (int)(imageWidth / aspectRatio);
const int samplesPerPixel = 100;
const int maxDepth = 50;

// World
var materialGround = new Lambertian(new Vec3(0.8, 0.8, 0));
var materialCenter = new Lambertian(new Vec3(0.1, 0.2, 0.5));
var materialLeft = new Dielectric(1.5);
var materialRight = new Metal(new Vec3(0.8, 0.6, 0.2), 0);

var world = new HittableList
{
    new Sphere(new Vec3(0, -100.5, -1), 100, materialGround),
    new Sphere(new Vec3(0, 0, -1), 0.5, materialCenter),
    new Sphere(new Vec3(-1, 0, -1), 0.5, materialLeft),
    new Sphere(new Vec3(1, 0, -1), 0.5, materialRight)
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
            pixelColor += RayColor(r, world, maxDepth);
        }
        
        pixelColor.WriteColor(Console.Out, samplesPerPixel);
    }
}

Console.Error.Write("\nDone.\n");

Vec3 RayColor(Ray r, IHittable world, int depth)
{
    if (depth <= 0) return new Vec3(0, 0, 0);
    
    var rec = new Hit();
    if (world.Hit(r, 0.001, double.PositiveInfinity, ref rec))
    {
        if (rec.Material.Scatter(r, rec, out var attenuation, out var scattered))
            return attenuation * RayColor(scattered, world, depth - 1);

        return new Vec3(0, 0, 0);
    }
    
    var unitDirection = Vec3.UnitVector(r.Direction);
    var t = 0.5 * (unitDirection.Y + 1);
    return (1 - t) * new Vec3(1, 1, 1) + t * new Vec3(0.5, 0.7, 1);
}