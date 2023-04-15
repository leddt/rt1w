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
    aspectRatio: 1, // 16.0 / 9.0,
    imageWidth: 800,
    samplesPerPixel: 10000,
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
    {
        var lookFrom = new Vec3(26, 3, 6);
        var lookAt = new Vec3(0, 2, 0);
        camera = new Camera(
            lookFrom,
            lookAt,
            vUp: new Vec3(0, 1, 0),
            vfov: 20,
            renderSettings.AspectRatio,
            aperture: 0.1,
            focusDist: 20,
            0, 1);

        scene = SimpleLight();
        background = new Vec3(0, 0, 0);
        break;
    }
    
    case 6:
    {
        var lookFrom = new Vec3(278, 278, -800);
        var lookAt = new Vec3(278, 278, 0);
        camera = new Camera(
            lookFrom,
            lookAt,
            vUp: new Vec3(0, 1, 0),
            vfov: 40,
            renderSettings.AspectRatio,
            aperture: 0.1,
            focusDist: 600,
            0, 1);

        scene = CornellBox();
        background = new Vec3(0, 0, 0);
        break;
    }
    
    case 7:
    {
        var lookFrom = new Vec3(278, 278, -800);
        var lookAt = new Vec3(278, 278, 0);
        camera = new Camera(
            lookFrom,
            lookAt,
            vUp: new Vec3(0, 1, 0),
            vfov: 40,
            renderSettings.AspectRatio,
            aperture: 0.1,
            focusDist: 600,
            0, 1);

        scene = CornellSmoke();
        background = new Vec3(0, 0, 0);
        break;
    }

    case 8:
    default:
    {
        var lookFrom = new Vec3(478, 278, -600);
        var lookAt = new Vec3(278, 278, 0);
        camera = new Camera(
            lookFrom,
            lookAt,
            vUp: new Vec3(0, 1, 0),
            vfov: 40,
            renderSettings.AspectRatio,
            aperture: 0.1,
            focusDist: 600,
            0, 1);
        
        scene = FinalScene();
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

IHittable SimpleLight()
{
    var world = new HittableList();
    
    var texture = new NoiseTexture(4);
    world.Add(new Sphere(new Vec3(0, -1000, 0), 1000, new Lambertian(texture)));
    world.Add(new Sphere(new Vec3(0, 2, 0), 2, new Lambertian(texture)));

    var diffLight = new DiffuseLight(new Vec3(4, 4, 4));
    world.Add(new RectXY(3, 5, 1, 3, -2, diffLight));
    world.Add(new Sphere(new Vec3(0, 7, 0), 2, diffLight));

    return world;
}

HittableList EmptyCornellBox()
{
    var objects = new HittableList();

    var red = new Lambertian(new Vec3(.65, .05, .05));
    var white = new Lambertian(new Vec3(.73, .73, .73));
    var green = new Lambertian(new Vec3(.13, .45, .15));
    var light = new DiffuseLight(new Vec3(7, 7, 7));

    objects.Add(new RectYZ(0, 555, 0, 555, 555, green));
    objects.Add(new RectYZ(0, 555, 0, 555, 0, red));
    objects.Add(new RectXZ(113, 443, 127, 432, 554, light));
    objects.Add(new RectXZ(0, 555, 0, 555, 0, white));
    objects.Add(new RectXZ(0, 555, 0, 555, 555, white));
    objects.Add(new RectXY(0, 555, 0, 555, 555, white));
    
    return objects;
}

IHittable CornellBox()
{
    var world = EmptyCornellBox();
    
    var white = new Lambertian(new Vec3(.73, .73, .73));

    world.Add(new Box(new Vec3(0, 0, 0), new Vec3(165, 330, 165), white)
        .RotateY(15)
        .Translate(265, 0, 295)
    );

    world.Add(new Box(new Vec3(0, 0, 0), new Vec3(165, 165, 165), white)
        .RotateY(-18)
        .Translate(130, 0, 65)
    );

    return world;
}

IHittable CornellSmoke()
{
    var world = EmptyCornellBox();
    
    var white = new Lambertian(new Vec3(.73, .73, .73));

    var box1 = new Box(new Vec3(0, 0, 0), new Vec3(165, 330, 165), white)
        .RotateY(15)
        .Translate(265, 0, 295);
    world.Add(new ConstantMedium(box1, 0.01, new Vec3(0, 0, 0)));

    var box2 = new Box(new Vec3(0, 0, 0), new Vec3(165, 165, 165), white)
        .RotateY(-18)
        .Translate(130, 0, 65);
    world.Add(new ConstantMedium(box2, 0.01, new Vec3(1, 1, 1)));

    return world;
}

IHittable FinalScene()
{
    var boxes1 = new HittableList();
    var ground = new Lambertian(new Vec3(0.48, 0.83, 0.53));

    var boxesPerSide = 20;
    for (var i = 0; i < boxesPerSide; i++)
    for (var j = 0; j < boxesPerSide; j++)
    {
        var w = 100.0;
        var x0 = -1000 + i * w;
        var z0 = -1000 + j * w;
        var y0 = 0;
        var x1 = x0 + w;
        var y1 = Random.Shared.NextDouble(1, 101);
        var z1 = z0 + w;

        boxes1.Add(new Box(new Vec3(x0, y0, z0), new Vec3(x1, y1, z1), ground));
    }

    var objects = new HittableList();
    
    objects.Add(new BvhNode(boxes1, 0, 1));

    var light = new DiffuseLight(new Vec3(7, 7, 7));
    objects.Add(new RectXZ(123, 423, 147, 412, 554, light));

    var center1 = new Vec3(400, 400, 200);
    var center2 = center1 + new Vec3(30, 0, 0);
    var movingSphereMaterial = new Lambertian(new Vec3(0.7, 0.3, 0.1));
    objects.Add(new MovingSphere(center1, center2, 0, 1, 50, movingSphereMaterial));

    objects.Add(new Sphere(new Vec3(260, 150, 45), 50, new Dielectric(1.5)));
    objects.Add(new Sphere(new Vec3(0, 150, 145), 50, new Metal(new Vec3(0.8, 0.8, 0.9), 1)));

    var boundary = new Sphere(new Vec3(360, 150, 145), 70, new Dielectric(1.5));
    objects.Add(boundary);
    objects.Add(new ConstantMedium(boundary, 0.2, new Vec3(0.2, 0.4, 0.9)));
    boundary = new Sphere(new Vec3(0, 0, 0), 5000, new Dielectric(1.5));
    objects.Add(new ConstantMedium(boundary, .0001, new Vec3(1, 1, 1)));

    var emat = new Lambertian(new ImageTexture("Textures/earthmap.jpg"));
    objects.Add(new Sphere(new Vec3(400, 200, 400), 100, emat));
    var pertext = new NoiseTexture(0.1);
    objects.Add(new Sphere(new Vec3(220, 280, 300), 80, new Lambertian(pertext)));

    var boxes2 = new HittableList();
    var white = new Lambertian(new Vec3(0.73, 0.73, 0.73));
    var ns = 1000;
    for (var j = 0; j < ns; j++) 
        boxes2.Add(new Sphere(Vec3.Random(0, 165), 10, white));

    objects.Add(new BvhNode(boxes2, 0, 1).RotateY(15).Translate(-100, 270, 395));

    return objects;
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