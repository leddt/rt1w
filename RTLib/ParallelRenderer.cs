using RTLib.Hittables;
using RTLib.Model;

namespace RTLib;

public class ParallelRenderer : Renderer
{
    public ParallelRenderer(RenderSettings settings) : base(settings)
    {
    }

    public Vec3[,] RenderScene(Vec3 background, IHittable scene, Camera camera, Action<string>? log)
    {
        log ??= _ => { };
        
        var canvas = new Vec3[Settings.ImageWidth, Settings.ImageHeight];
        var linesRemaining = Settings.ImageHeight;
        
        Parallel.For(0, Settings.ImageHeight, j =>
        {
            for (var i = 0; i < Settings.ImageWidth; i++)
            {
                var pixelColor = Vec3.Zero;

                for (var s = 0; s < Settings.SamplesPerPixel; s++)
                {
                    pixelColor += GetSample(background, scene, camera, i, j);
                }

                canvas[i, j] = pixelColor.ToRgb(Settings.SamplesPerPixel);
            }
    
            log($"\rLines: {Interlocked.Decrement(ref linesRemaining)} ");
        });

        return canvas;
    }
}