using RTLib;
using RTLib.Hittables;
using RTLib.Materials;
using RTLib.Model;
using RTLib.Textures;

namespace RTConsole.Scenes;

public class FinalScene : IScene
{
    public Vec3 GetBackground() => new(0, 0, 0);

    public RenderSettings GetRenderSettings() => new(
        aspectRatio: 1,
        imageWidth: 800,
        samplesPerPixel: 500, // 10000,
        maxDepth: 50
    );

    public Camera GetCamera() => new(
        lookFrom: new Vec3(478, 278, -600),
        lookAt: new Vec3(278, 278, 0),
        vUp: new Vec3(0, 1, 0),
        vfov: 40,
        GetRenderSettings().AspectRatio,
        aperture: 0.1,
        focusDist: 600,
        time0: 0, time1: 1);

    public IHittable GetWorld()
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
}