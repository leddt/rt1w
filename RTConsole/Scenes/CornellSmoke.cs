using RTLib;
using RTLib.Hittables;
using RTLib.Materials;
using RTLib.Model;

namespace RTConsole.Scenes;

public class CornellSmoke : IScene
{
    public Vec3 GetBackground() => new(0, 0, 0);

    public RenderSettings GetRenderSettings() => new(
        aspectRatio: 1,
        imageWidth: 800,
        samplesPerPixel: 500,
        maxDepth: 50
    );

    public Camera GetCamera() => new(
        lookFrom: new Vec3(278, 278, -800),
        lookAt: new Vec3(278, 278, 0),
        vUp: new Vec3(0, 1, 0),
        vfov: 40,
        GetRenderSettings().AspectRatio,
        aperture: 0.1,
        focusDist: 600,
        time0: 0, time1: 1);

    public IHittable GetWorld()
    {
        var world = new HittableList();

        var red = new Lambertian(new Vec3(.65, .05, .05));
        var white = new Lambertian(new Vec3(.73, .73, .73));
        var green = new Lambertian(new Vec3(.13, .45, .15));
        var light = new DiffuseLight(new Vec3(7, 7, 7));

        world.Add(new RectYZ(0, 555, 0, 555, 555, green));
        world.Add(new RectYZ(0, 555, 0, 555, 0, red));
        world.Add(new RectXZ(113, 443, 127, 432, 554, light));
        world.Add(new RectXZ(0, 555, 0, 555, 0, white));
        world.Add(new RectXZ(0, 555, 0, 555, 555, white));
        world.Add(new RectXY(0, 555, 0, 555, 555, white));
    
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
}