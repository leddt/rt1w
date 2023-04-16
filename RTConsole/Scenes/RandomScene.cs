using RTLib;
using RTLib.Hittables;
using RTLib.Materials;
using RTLib.Model;
using RTLib.Textures;

namespace RTConsole.Scenes;

public class RandomScene : IScene
{
    public Vec3 GetBackground() => new(0.7, 0.8, 1);

    public RenderSettings GetRenderSettings() => new(
        aspectRatio: 16.0 / 9.0,
        imageWidth: 800,
        samplesPerPixel: 500,
        maxDepth: 50
    );

    public Camera GetCamera() => new(
        lookFrom: new Vec3(13, 2, 3),
        lookAt: new Vec3(0, 0, 0),
        vUp: new Vec3(0, 1, 0),
        vfov: 20,
        GetRenderSettings().AspectRatio,
        aperture: 0.1,
        focusDist: 10,
        time0: 0, time1: 1
    );

    public IHittable GetWorld()
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
}