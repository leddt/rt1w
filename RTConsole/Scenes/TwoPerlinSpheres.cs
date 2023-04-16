using RTLib;
using RTLib.Hittables;
using RTLib.Materials;
using RTLib.Textures;

namespace RTConsole.Scenes;

public class TwoPerlinSpheres : BaseScene
{
    public override Vec3 GetBackground() => new(0.7, 0.8, 1);

    protected override Vec3 LookFrom => new(13, 2, 3);
    protected override Vec3 LookAt => Vec3.Zero;
    protected override double VerticalFov => 20;

    protected override IEnumerable<IHittable> GetSceneObjects()
    {
        var texture = new NoiseTexture(4);

        yield return new Sphere(new Vec3(0, -1000, 0), 1000, new Lambertian(texture));
        yield return new Sphere(new Vec3(0, 2, 0), 2, new Lambertian(texture));
    }
}