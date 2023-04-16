using RTLib;
using RTLib.Hittables;
using RTLib.Materials;
using RTLib.Model;
using RTLib.Textures;

namespace RTConsole.Scenes;

public class Earth : IScene
{
    public Vec3 GetBackground() => new(0.7, 0.8, 1);

    public RenderSettings GetRenderSettings() => new(
        aspectRatio: 1,
        imageWidth: 800,
        samplesPerPixel: 500,
        maxDepth: 50
    );

    public Camera GetCamera()
    {
        return new Camera(
            lookFrom: new Vec3(13, 2, 3),
            lookAt: new Vec3(0, 0, 0),
            vUp: new Vec3(0, 1, 0),
            vfov: 20,
            GetRenderSettings().AspectRatio,
            aperture: 0.1,
            focusDist: 10,
            time0: 0, time1: 1);
    }

    public IHittable GetWorld()
    {
        var earthTexture = new ImageTexture("Textures/earthmap.jpg");
        var earthSurface = new Lambertian(earthTexture);
        var globe = new Sphere(new Vec3(0, 0, 0), 2, earthSurface);

        return globe;
    }
}