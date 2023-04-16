using System.Runtime.CompilerServices;
using RTLib.Hittables;
using RTLib.Model;

namespace RTLib;

public abstract class Renderer
{
    protected readonly RenderSettings Settings;

    protected Renderer(RenderSettings settings)
    {
        Settings = settings;
    }

    private static Vec3 RayColor(Ray r, Vec3 background, IHittable scene, int depth)
    {
        if (depth <= 0) return new Vec3(0, 0, 0);
        
        var rec = new Hit();
        if (!scene.Hit(r, 0.001, double.PositiveInfinity, ref rec))
            return background;

        var emitted = rec.Material.Emitted(rec.U, rec.V, rec.P);

        if (!rec.Material.Scatter(r, rec, out var attenuation, out var scattered))
            return emitted;

        return emitted + attenuation * RayColor(scattered, background, scene, depth - 1);
    }

    protected Vec3 GetSample(Vec3 background, IHittable scene, Camera camera, int i, int j)
    {
        var u = (i + Random.Shared.NextDouble()) / (Settings.ImageWidth - 1);
        var v = (j + Random.Shared.NextDouble()) / (Settings.ImageHeight - 1);
        var r = camera.GetRay(u, v);
        var sample = RayColor(r, background, scene, Settings.MaxDepth);
        return sample;
    }
}

public struct RenderSettings
{
    public RenderSettings(double aspectRatio, int imageWidth, int samplesPerPixel, int maxDepth)
    {
        AspectRatio = aspectRatio;
        ImageWidth = imageWidth;
        ImageHeight = (int)(imageWidth / aspectRatio);
        SamplesPerPixel = samplesPerPixel;
        MaxDepth = maxDepth;
    }

    public readonly double AspectRatio;
    public readonly int ImageWidth;
    public readonly int ImageHeight;
    public readonly int SamplesPerPixel;
    public readonly int MaxDepth;
}