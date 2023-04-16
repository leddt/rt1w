using RTLib;
using RTLib.Hittables;
using RTLib.Materials;
using RTLib.Textures;

namespace RTConsole.Scenes;

public class FinalScene : BaseScene
{
    protected override double AspectRatio => 1;
    protected override Vec3 LookFrom => new(478, 278, -600);
    protected override Vec3 LookAt => new(278, 278, 0);
    
    protected override IEnumerable<IHittable> GetSceneObjects()
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

        yield return new BvhNode(boxes1, 0, 1);

        var light = new DiffuseLight(new Vec3(7, 7, 7));
        yield return new RectXZ(123, 423, 147, 412, 554, light);

        var center1 = new Vec3(400, 400, 200);
        var center2 = center1 + new Vec3(30, 0, 0);
        var movingSphereMaterial = new Lambertian(new Vec3(0.7, 0.3, 0.1));
        yield return new MovingSphere(center1, center2, 0, 1, 50, movingSphereMaterial);

        yield return new Sphere(new Vec3(260, 150, 45), 50, new Dielectric(1.5));
        yield return new Sphere(new Vec3(0, 150, 145), 50, new Metal(new Vec3(0.8, 0.8, 0.9), 1));

        var boundary = new Sphere(new Vec3(360, 150, 145), 70, new Dielectric(1.5));
        yield return boundary;
        yield return new ConstantMedium(boundary, 0.2, new Vec3(0.2, 0.4, 0.9));
        boundary = new Sphere(Vec3.Zero, 5000, new Dielectric(1.5));
        yield return new ConstantMedium(boundary, .0001, new Vec3(1, 1, 1));

        var emat = new Lambertian(new ImageTexture("Textures/earthmap.jpg"));
        yield return new Sphere(new Vec3(400, 200, 400), 100, emat);
        var pertext = new NoiseTexture(0.1);
        yield return new Sphere(new Vec3(220, 280, 300), 80, new Lambertian(pertext));

        var boxes2 = new HittableList();
        var white = new Lambertian(new Vec3(0.73, 0.73, 0.73));
        var ns = 1000;
        for (var j = 0; j < ns; j++)
            boxes2.Add(new Sphere(Vec3.Random(0, 165), 10, white));

        yield return new BvhNode(boxes2, 0, 1).RotateY(15).Translate(-100, 270, 395);
    }
}