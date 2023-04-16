using RTLib;
using RTLib.Hittables;
using RTLib.Materials;

namespace RTConsole.Scenes;

public class CornellBox : BaseScene
{
    public override Vec3 GetBackground() => new Vec3(0.12, 0.15, 0.50);

    protected override double AspectRatio => 1;
    protected override Vec3 LookFrom => new(278, 278, -800);
    protected override Vec3 LookAt => new(278, 278, 0);
    protected override int SamplesPerPixel => 2000;

    protected override IEnumerable<IHittable> GetSceneObjects()
    {
        var red = new Lambertian(new Vec3(.65, .05, .05));
        var white = new Lambertian(new Vec3(.73, .73, .73));
        var green = new Lambertian(new Vec3(.12, .45, .15));
        var light = new DiffuseLight(new Vec3(15, 15, 15));

        yield return new RectYZ(0, 555, 0, 555, 555, green);
        yield return new RectYZ(0, 555, 0, 555, 0, red);
        yield return new RectXZ(213, 343, 227, 332, 554, light);
        yield return new RectXZ(0, 555, 0, 555, 0, white);
        yield return new RectXZ(0, 555, 0, 555, 555, white);
        yield return new RectXY(0, 555, 0, 555, 555, white);

        var metal = new Metal(new Vec3(0.7, 0.7, 0.7), 0.05);
        yield return new Box(Vec3.Zero, new Vec3(165, 330, 165), metal)
            .RotateY(15)
            .RotateX(15)
            .RotateZ(15)
            .Translate(265, 0, 295);

        var glass = new Dielectric(1.5);
        yield return new Box(Vec3.Zero, new Vec3(165, 165, 165), glass)
            .RotateY(-18)
            .Translate(130, 0, 65);
    }
}