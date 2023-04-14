using RTLib.Hittables;

namespace RTLib;

public class ParallelRenderer
{
    private readonly RenderSettings _settings;

    public ParallelRenderer(RenderSettings settings)
    {
        _settings = settings;
    }

    public Vec3[,] RenderScene(IHittable scene, Camera camera, Action<string>? log)
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
                    pixelColor += RayColor(r, scene, _settings.MaxDepth);
                }

                canvas[i, j] = pixelColor.ToRGB(_settings.SamplesPerPixel);
            }
    
            log($"\rLines: {Interlocked.Decrement(ref linesRemaining)} ");
        });

        return canvas;
    }
    
    private Vec3 RayColor(Ray r, IHittable scene, int depth)
    {
        if (depth <= 0) return new Vec3(0, 0, 0);
    
        var rec = new Hit();
        if (scene.Hit(r, 0.001, double.PositiveInfinity, ref rec))
        {
            if (rec.Material.Scatter(r, rec, out var attenuation, out var scattered))
                return attenuation * RayColor(scattered, scene, depth - 1);

            return new Vec3(0, 0, 0);
        }
    
        var unitDirection = Vec3.UnitVector(r.Direction);
        var t = 0.5 * (unitDirection.Y + 1);
        return (1 - t) * new Vec3(1, 1, 1) + t * new Vec3(0.5, 0.7, 1);
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