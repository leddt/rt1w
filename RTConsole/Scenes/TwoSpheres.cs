using RTLib;
using RTLib.Hittables;
using RTLib.Materials;
using RTLib.Model;
using RTLib.Textures;

namespace RTConsole.Scenes;

public class TwoSpheres : IScene
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
        lookAt: Vec3.Zero,
        vUp: new Vec3(0, 1, 0),
        vfov: 20,
        GetRenderSettings().AspectRatio,
        aperture: 0.1,
        focusDist: 10,
        time0: 0, time1: 1
    );

    public IHittable GetWorld()
    {
        var world = new HittableList();
    
        var checker = new CheckerTexture(new Vec3(0.2, 0.3, 0.1), new Vec3(0.9, 0.9, 0.9));

        world.Add(new Sphere(new Vec3(0, -10, 0), 10, new Lambertian(checker)));
        world.Add(new Sphere(new Vec3(0, 10, 0), 10, new Lambertian(checker)));

        return world;
    }
}