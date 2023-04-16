using RTLib;
using RTLib.Hittables;
using RTLib.Materials;
using RTLib.Model;
using RTLib.Textures;

namespace RTConsole.Scenes;

public class SimpleLight : IScene
{
    public Vec3 GetBackground() => new(0, 0, 0);

    public RenderSettings GetRenderSettings() => new(
        aspectRatio: 16.0 / 9.0,
        imageWidth: 800,
        samplesPerPixel: 500,
        maxDepth: 50
    );

    public Camera GetCamera()
    {
        return new Camera(
            lookFrom: new Vec3(26, 3, 6),
            lookAt: new Vec3(0, 2, 0),
            vUp: new Vec3(0, 1, 0),
            vfov: 20,
            GetRenderSettings().AspectRatio,
            aperture: 0.1,
            focusDist: 20,
            time0: 0, time1: 1);
    }

    public IHittable GetWorld()
    {
        var world = new HittableList();
    
        var texture = new NoiseTexture(4);
        world.Add(new Sphere(new Vec3(0, -1000, 0), 1000, new Lambertian(texture)));
        world.Add(new Sphere(new Vec3(0, 2, 0), 2, new Lambertian(texture)));

        var diffLight = new DiffuseLight(new Vec3(4, 4, 4));
        world.Add(new RectXY(3, 5, 1, 3, -2, diffLight));
        world.Add(new Sphere(new Vec3(0, 7, 0), 2, diffLight));

        return world;
    }
}