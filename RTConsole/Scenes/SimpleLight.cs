using RTLib;
using RTLib.Hittables;
using RTLib.Materials;
using RTLib.Textures;

namespace RTConsole.Scenes;

public class SimpleLight : BaseScene
{
    protected override Vec3 LookFrom => new(26, 3, 6);
    protected override Vec3 LookAt => new(0, 2, 0);
    protected override double VerticalFov => 20;
    protected override int SamplesPerPixel => 2000;

    protected override IEnumerable<IHittable> GetSceneObjects()
    {
        var texture = new NoiseTexture(4);
        yield return new Sphere(new Vec3(0, -1000, 0), 1000, new Lambertian(texture));
        yield return new Sphere(new Vec3(0, 2, 0), 2, new Lambertian(texture));

        var diffLight = new DiffuseLight(new Vec3(4, 4, 4));
        yield return new RectXY(3, 5, 1, 3, -2, diffLight);
        yield return new Sphere(new Vec3(0, 7, 0), 2, diffLight);
    }
}