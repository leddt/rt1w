using RTLib;
using RTLib.Formats;
using RTLib.Hittables;
using RTLib.Materials;
using RTLib.Model;

var renderSettings = new RenderSettings(
    aspectRatio: 3.0 / 2.0,
    imageWidth: 600,
    samplesPerPixel: 100,
    maxDepth: 50
);

var scene = RandomScene();

// Camera
var lookFrom = new Vec3(13, 2, 3);
var lookAt = new Vec3(0, 0, 0);
var camera = new Camera(
    lookFrom, 
    lookAt, 
    vUp: new Vec3(0, 1, 0), 
    vfov: 20, 
    renderSettings.AspectRatio,
    aperture: 0.1,
    focusDist: 10);

// Render
Console.Error.WriteLine("Rendering...");

var renderer = new ParallelRenderer(renderSettings);
var pixels = renderer.RenderScene(scene, camera, Console.Error.Write);


IFormat fileFormat;
Stream targetStream;

var commandLine = Environment.GetCommandLineArgs();
if (commandLine.Length >= 2)
{
    var targetFileName = Environment.GetCommandLineArgs()[1];
    
    if (targetFileName.EndsWith(".png"))
        fileFormat = new PngFormat();
    else if (targetFileName.EndsWith(".ppm"))
        fileFormat = new PpmFormat();
    else
        throw new Exception($"Unsupported file format: {targetFileName}");

    targetStream = File.Create(targetFileName);
}
else
{
    fileFormat = new PpmFormat();
    targetStream = Console.OpenStandardOutput();
}

// Output
Console.Error.Write("Writing file... ");

using (targetStream)
{
    fileFormat.WriteFile(targetStream, pixels);
}

Console.Error.Write("Done.\n");

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