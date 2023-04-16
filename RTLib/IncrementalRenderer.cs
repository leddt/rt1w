using RTLib.Hittables;
using RTLib.Model;

namespace RTLib;

public class IncrementalRenderer : Renderer
{
    public IncrementalRenderer(RenderSettings settings) : base(settings)
    {
    }

    public void RenderScene(Vec3 background, IHittable scene, Camera camera, Action<Vec3[,]> frameReady, Action<string>? log)
    {
        log ??= _ => { };
        
        var canvas = new Vec3[Settings.ImageWidth, Settings.ImageHeight];
        var samplesRemaining = Settings.SamplesPerPixel;
        
        for (var s = 0; s < Settings.SamplesPerPixel; s++)
        {
            Parallel.For(0, Settings.ImageHeight, j =>
            {
                for (var i = 0; i < Settings.ImageWidth; i++)
                {
                    canvas[i, j] += GetSample(background, scene, camera, i, j);
                }
            });

            if (s % 10 == 0)
            {
                frameReady(GetFrame(canvas, s + 1));
            }

            log($"\r{Interlocked.Decrement(ref samplesRemaining)} samples remaining ");
        }
    }

    private Vec3[,] GetFrame(Vec3[,] canvas, int samples)
    {
        var result = new Vec3[Settings.ImageWidth, Settings.ImageHeight];

        for (var x = 0; x < Settings.ImageWidth; x++)
        for (var y = 0; y < Settings.ImageHeight; y++)
        {
            result[x, y] = canvas[x, y].ToRgb(samples);
        }
        
        return result;
    }
}