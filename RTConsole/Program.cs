using System.Diagnostics;
using RTLib;
using RTLib.Formats;
using RTLib.Hittables;
using RTLib.Materials;
using RTLib.Model;
using RTLib.Textures;

var sw = Stopwatch.StartNew();

var commandLine = Environment.GetCommandLineArgs();

var renderSettings = new RenderSettings(
    aspectRatio: 16.0 / 9.0,
    imageWidth: 600,
    samplesPerPixel: 200,
    maxDepth: 50
);

// Setup scene
if (commandLine.Length < 3 || !int.TryParse(commandLine[2], out var sceneIndex))
    sceneIndex = 0;

Camera camera;
IHittable scene;
Vec3 background;

switch (sceneIndex)
{
    case 1:
    {
        var lookFrom = new Vec3(13, 2, 3);
        var lookAt = new Vec3(0, 0, 0);
        camera = new Camera(
            lookFrom,
            lookAt,
            vUp: new Vec3(0, 1, 0),
            vfov: 20,
            renderSettings.AspectRatio,
            aperture: 0.1,
            focusDist: 10,
            0, 1);

        scene = RandomScene();
        background = new Vec3(0.7, 0.8, 1);
        break;
    }
    
    case 2:
    {
        var lookFrom = new Vec3(13, 2, 3);
        var lookAt = new Vec3(0, 0, 0);
        camera = new Camera(
            lookFrom,
            lookAt,
            vUp: new Vec3(0, 1, 0),
            vfov: 20,
            renderSettings.AspectRatio,
            aperture: 0.1,
            focusDist: 10,
            0, 1);

        scene = TwoSpheres();
        background = new Vec3(0.7, 0.8, 1);
        break;
    }

    case 3:
    {
        var lookFrom = new Vec3(13, 2, 3);
        var lookAt = new Vec3(0, 0, 0);
        camera = new Camera(
            lookFrom,
            lookAt,
            vUp: new Vec3(0, 1, 0),
            vfov: 20,
            renderSettings.AspectRatio,
            aperture: 0.1,
            focusDist: 10,
            0, 1);

        scene = TwoPerlinSpheres();
        background = new Vec3(0.7, 0.8, 1);
        break;
    }
    
    case 4:
    {
        var lookFrom = new Vec3(13, 2, 3);
        var lookAt = new Vec3(0, 0, 0);
        camera = new Camera(
            lookFrom,
            lookAt,
            vUp: new Vec3(0, 1, 0),
            vfov: 20,
            renderSettings.AspectRatio,
            aperture: 0.1,
            focusDist: 10,
            0, 1);

        scene = Earth();
        background = new Vec3(0.7, 0.8, 1);
        break;
    }
    
    case 5:
    default:
    {
        var lookFrom = new Vec3(13, 2, 3);
        var lookAt = new Vec3(0, 0, 0);
        camera = new Camera(
            lookFrom,
            lookAt,
            vUp: new Vec3(0, 1, 0),
            vfov: 20,
            renderSettings.AspectRatio,
            aperture: 0.1,
            focusDist: 10,
            0, 1);

        scene = new HittableList();
        background = new Vec3(0, 0, 0);
        break;
    }
}

// Render
Console.Error.WriteLine("Rendering...");
var renderer = new ParallelRenderer(renderSettings);
var pixels = renderer.RenderScene(background, scene, camera, Console.Error.Write);

// Output
Console.Error.Write("Writing file... ");
using (var outputStream = GetOutputStream(out var format))
{
    format.WriteFile(outputStream, pixels);
}

Console.Error.Write($"Done ({sw.Elapsed})\n");

IHittable RandomScene()
{
    var t0 = 0.0;
    var t1 = 1.0;
    
    var world = new HittableList();

    var checker = new CheckerTexture(new Vec3(0.2, 0.3, 0.1), new Vec3(0.9, 0.9, 0.9));
    var groundMaterial = new Lambertian(checker);
    world.Add(new Sphere(new Vec3(0, -1000, 0), 1000, groundMaterial));

    var rng = Random.Shared;
    
    for (var a = -11; a < 11; a++)
    for (var b = -11; b < 11; b++)
    {
        var chooseMat = rng.NextDouble();
        var center = new Vec3(a + 0.9 * rng.NextDouble(), 0.2, b + 0.9 * rng.NextDouble());
    
        if ((center - new Vec3(4, 0.2, 0)).Length > 0.9)
        {
            if (chooseMat < 0.8)
            {
                // diffuse
                var albedo = Vec3.Random() * Vec3.Random();
                var mat = new Lambertian(albedo);
                var center2 = center + new Vec3(0, rng.NextDouble(0, 0.5), 0);
            
                world.Add(new MovingSphere(center, center2, t0, t1, 0.2, mat));
            }
            else if (chooseMat < 0.95)
            {
                // metal
                var albedo = Vec3.Random(0.5, 1);
                var fuz = rng.NextDouble(0, 0.5);
                var mat = new Metal(albedo, fuz);
            
                world.Add(new Sphere(center, 0.2, mat));
            }
            else
            {
                // glass
                var mat = new Dielectric(1.5);
            
                world.Add(new Sphere(center, 0.2, mat));
            }
        }
    }
    
    world.Add(new Sphere(new Vec3(0, 1, 0), 1, new Dielectric(1.5)));
    world.Add(new Sphere(new Vec3(-4, 1, 0), 1, new Lambertian(new Vec3(0.4, 0.2, 0.1))));
    world.Add(new Sphere(new Vec3(4, 1, 0), 1, new Metal(new Vec3(0.7, 0.6, 0.5), 0)));

    return new BvhNode(world, t0, t1);
}

IHittable TwoSpheres()
{
    var world = new HittableList();
    
    var checker = new CheckerTexture(new Vec3(0.2, 0.3, 0.1), new Vec3(0.9, 0.9, 0.9));

    world.Add(new Sphere(new Vec3(0, -10, 0), 10, new Lambertian(checker)));
    world.Add(new Sphere(new Vec3(0, 10, 0), 10, new Lambertian(checker)));

    return world;
}

IHittable TwoPerlinSpheres()
{
    var world = new HittableList();

    var texture = new NoiseTexture(4);

    world.Add(new Sphere(new Vec3(0, -1000, 0), 1000, new Lambertian(texture)));
    world.Add(new Sphere(new Vec3(0, 2, 0), 2, new Lambertian(texture)));

    return world;
}

IHittable Earth()
{
    var earthTexture = new ImageTexture("Textures/earthmap.jpg");
    var earthSurface = new Lambertian(earthTexture);
    var globe = new Sphere(new Vec3(0, 0, 0), 2, earthSurface);

    return globe;
}

Stream GetOutputStream(out IFormat format)
{
    if (commandLine.Length >= 2)
    {
        var targetFileName = Environment.GetCommandLineArgs()[1];

        if (targetFileName.EndsWith(".png"))
            format = new PngFormat();
        else if (targetFileName.EndsWith(".ppm"))
            format = new PpmFormat();
        else
            throw new Exception($"Unsupported file format: {targetFileName}");

        return File.Create(targetFileName);
    }

    format = new PpmFormat();
    return Console.OpenStandardOutput();
}