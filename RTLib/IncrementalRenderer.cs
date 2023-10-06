using RTLib.Hittables;
using RTLib.Model;

namespace RTLib;

public class IncrementalRenderer : Renderer
{
    private const int BatchSize = 10;
    
    public IncrementalRenderer(RenderSettings settings) : base(settings)
    {
    }

    public void RenderScene(Vec3 background, IHittable scene, Camera camera, Action<Vec3[,]> frameReady, Action<string>? log)
    {
        log ??= _ => { };
        
        var canvas = new Vec3[Settings.ImageWidth, Settings.ImageHeight];
        var samplesRemaining = Settings.SamplesPerPixel;
        var batchCount = (Settings.SamplesPerPixel + BatchSize - 1) / BatchSize;
        
        for (var b = 0; b < batchCount; b++)
        {
            Parallel.For(0, Settings.ImageHeight, j =>
            {
                for (var s = 0; s < BatchSize; s++)
                for (var i = 0; i < Settings.ImageWidth; i++)
                {
                    canvas[i, j] += GetSample(background, scene, camera, i, j);
                }
            });

            frameReady(GetFrame(canvas, (b + 1) * BatchSize));

            log($"\r{Interlocked.Add(ref samplesRemaining, -BatchSize)} samples remaining ");
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