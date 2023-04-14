using RTConsole;
using RTConsole.Hittables;
using RTConsole.Materials;

// Image
const double aspectRatio = 3.0 / 2.0;
const int imageWidth = 1200;
const int imageHeight = (int)(imageWidth / aspectRatio);
const int samplesPerPixel = 500;
const int maxDepth = 50;

// World
var world = RandomScene();

// Camera
var lookFrom = new Vec3(13, 2, 3);
var lookAt = new Vec3(0, 0, 0);
var camera = new Camera(
    lookFrom, 
    lookAt, 
    vUp: new Vec3(0, 1, 0), 
    vfov: 20, 
    aspectRatio,
    aperture: 0.1,
    focusDist: 10);

// Render

var canvas = new Vec3[imageWidth, imageHeight];

Console.Error.WriteLine("Rendering...");

var linesRemaining = imageHeight;
Parallel.For(0, imageHeight, j =>
{
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

        canvas[i, j] = pixelColor;
    }
    
    Console.Error.Write($"\rLines: {Interlocked.Decrement(ref linesRemaining)} ");
});

Console.Error.Write("Writing file... ");

Console.WriteLine($"P3\n{imageWidth} {imageHeight}\n255");
for (var j = imageHeight - 1; j >= 0; j--)
for (var i = 0; i < imageWidth; i++)
    canvas[i, j].WriteColor(Console.Out, samplesPerPixel);

Console.Error.Write("Done.\n");

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

HittableList RandomScene()
{
    var world = new HittableList();

    var groundMaterial = new Lambertian(new Vec3(0.5, 0.5, 0.5));
    world.Add(new Sphere(new Vec3(0, -1000, 0), 1000, groundMaterial));

    var rng = Random.Shared;
    
    for (var a = -11; a < 11; a++)
    for (var b = -11; b < 11; b++)
    {
        var chooseMat = rng.NextDouble();
        var center = new Vec3(a + 0.9 * rng.NextDouble(), 0.2, b + 0.9 * rng.NextDouble());

        if ((center - new Vec3(4, 0.2, 0)).Length > 0.9)
        {
            IMaterial mat;
            
            if (chooseMat < 0.8)
            {
                // diffuse
                var albedo = Vec3.Random() * Vec3.Random();
                mat = new Lambertian(albedo);
            }
            else if (chooseMat < 0.95)
            {
                // metal
                var albedo = Vec3.Random(0.5, 1);
                var fuz = rng.NextDouble(0, 0.5);
                mat = new Metal(albedo, fuz);
            }
            else
            {
                // glass
                mat = new Dielectric(1.5);
            }
            
            world.Add(new Sphere(center, 0.2, mat));
        }
    }

    world.Add(new Sphere(new Vec3(0, 1, 0), 1, new Dielectric(1.5)));
    world.Add(new Sphere(new Vec3(-4, 1, 0), 1, new Lambertian(new Vec3(0.4, 0.2, 0.1))));
    world.Add(new Sphere(new Vec3(4, 1, 0), 1, new Metal(new Vec3(0.7, 0.6, 0.5), 0)));

    return world;
}