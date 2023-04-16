using RTLib;
using RTLib.Hittables;
using RTLib.Materials;
using RTLib.Textures;

namespace RTConsole.Scenes;

public class RandomScene : BaseScene
{
    public override Vec3 GetBackground() => new(0.7, 0.8, 1);
    protected override Vec3 LookFrom => new(13, 2, 3);
    protected override Vec3 LookAt => Vec3.Zero;
    protected override double VerticalFov => 20;
    protected override double Aperture => 0.1;
    protected override double FocusDistance => 10;

    protected override IEnumerable<IHittable> GetSceneObjects()
    {
        var checker = new CheckerTexture(new Vec3(0.2, 0.3, 0.1), new Vec3(0.9, 0.9, 0.9));
        var groundMaterial = new Lambertian(checker);
        yield return new Sphere(new Vec3(0, -1000, 0), 1000, groundMaterial);

        var smallSpheres = new HittableList();
        
        for (var a = -11; a < 11; a++)
        for (var b = -11; b < 11; b++)
        {
            var center = new Vec3(
                a + 0.9 * Random.Shared.NextDouble(),
                0.2,
                b + 0.9 * Random.Shared.NextDouble()
            );

            if ((center - new Vec3(4, 0.2, 0)).Length > 0.9)
            {
                smallSpheres.Add(GetRandomSphere(center));
            }
        }

        yield return new BvhNode(smallSpheres, TimeStart, TimeEnd);

        yield return new Sphere(new Vec3(0, 1, 0), 1, new Dielectric(1.5));
        yield return new Sphere(new Vec3(-4, 1, 0), 1, new Lambertian(new Vec3(0.4, 0.2, 0.1)));
        yield return new Sphere(new Vec3(4, 1, 0), 1, new Metal(new Vec3(0.7, 0.6, 0.5), 0));
    }

    private IHittable GetRandomSphere(Vec3 center)
    {
        var rng = Random.Shared;
        var chooseMat = rng.NextDouble();
        
        switch (chooseMat)
        {
            case < 0.8:
            {
                // diffuse
                var albedo = Vec3.Random() * Vec3.Random();
                var mat = new Lambertian(albedo);
                var center2 = center + new Vec3(0, rng.NextDouble(0, 0.5), 0);

                return new MovingSphere(center, center2, TimeStart, TimeEnd, 0.2, mat);
            }
            case < 0.95:
            {
                // metal
                var albedo = Vec3.Random(0.5, 1);
                var fuz = rng.NextDouble(0, 0.5);
                var mat = new Metal(albedo, fuz);

                return new Sphere(center, 0.2, mat);
            }
            default:
            {
                // glass
                var mat = new Dielectric(1.5);

                return new Sphere(center, 0.2, mat);
            }
        }
    }
}