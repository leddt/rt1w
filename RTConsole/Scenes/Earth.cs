using RTLib;
using RTLib.Hittables;
using RTLib.Materials;
using RTLib.Textures;

namespace RTConsole.Scenes;

public class Earth : BaseScene
{
    public override Vec3 GetBackground() => new(0.7, 0.8, 1);
    protected override double AspectRatio => 1;
    protected override double VerticalFov => 20;
    protected override Vec3 LookFrom => new (13, 2, 3);
    protected override Vec3 LookAt => Vec3.Zero;
    
    protected override IEnumerable<IHittable> GetSceneObjects()
    {
        var earthTexture = new ImageTexture("Textures/earthmap.jpg");
        var earthSurface = new Lambertian(earthTexture);
        yield return new Sphere(Vec3.Zero, 2, earthSurface);
    }
}