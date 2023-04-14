﻿using System.Diagnostics;
using RTLib;
using RTLib.Formats;
using RTLib.Hittables;
using RTLib.Materials;
using RTLib.Model;

var sw = Stopwatch.StartNew();

var renderSettings = new RenderSettings(
    aspectRatio: 16.0 / 9.0,
    imageWidth: 400,
    samplesPerPixel: 100,
    maxDepth: 50
);

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
    focusDist: 10,
    0, 1);

var scene = RandomScene();

// Render
Console.Error.WriteLine("Rendering...");
var renderer = new ParallelRenderer(renderSettings);
var pixels = renderer.RenderScene(scene, camera, Console.Error.Write);

// Output
Console.Error.Write("Writing file... ");
using (var outputStream = GetOutputStream(out var format))
{
    format.WriteFile(outputStream, pixels);
}

Console.Error.Write($"Done ({sw.Elapsed})\n");

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
            if (chooseMat < 0.8)
            {
                // diffuse
                var albedo = Vec3.Random() * Vec3.Random();
                var mat = new Lambertian(albedo);
                var center2 = center + new Vec3(0, rng.NextDouble(0, 0.5), 0);
            
                world.Add(new MovingSphere(center, center2, 0, 1, 0.2, mat));
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

    return world;
}

Stream GetOutputStream(out IFormat format)
{
    var commandLine = Environment.GetCommandLineArgs();
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