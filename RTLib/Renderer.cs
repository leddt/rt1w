using RTLib.Hittables;
using RTLib.Model;

namespace RTLib;

public class ParallelRenderer
{
    private readonly RenderSettings _settings;

    public ParallelRenderer(RenderSettings settings)
    {
        _settings = settings;
    }

    public Vec3[,] RenderScene(Vec3 background, IHittable scene, Camera camera, Action<string>? log)
    {
        log ??= _ => { };
        
        var canvas = new Vec3[_settings.ImageWidth, _settings.ImageHeight];
        var linesRemaining = _settings.ImageHeight;
        
        Parallel.For(0, _settings.ImageHeight, j =>
        {
            for (var i = 0; i < _settings.ImageWidth; i++)
            {
                var pixelColor = new Vec3(0, 0, 0);

                for (int s = 0; s < _settings.SamplesPerPixel; s++)
                {
                    var u = (i + Random.Shared.NextDouble()) / (_settings.ImageWidth - 1);
                    var v = (j + Random.Shared.NextDouble()) / (_settings.ImageHeight - 1);
                    var r = camera.GetRay(u, v);
                    pixelColor += RayColor(r, background, scene, _settings.MaxDepth);
                }

                canvas[i, j] = pixelColor.ToRGB(_settings.SamplesPerPixel);
            }
    
            log($"\rLines: {Interlocked.Decrement(ref linesRemaining)} ");
        });

        return canvas;
    }
    
    private Vec3 RayColor(Ray r, Vec3 background, IHittable scene, int depth)
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