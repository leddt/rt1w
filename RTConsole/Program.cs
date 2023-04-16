using System.Diagnostics;
using RTConsole.Scenes;
using RTLib;
using RTLib.Formats;

var sw = Stopwatch.StartNew();
var commandLine = Environment.GetCommandLineArgs();
var incremental = true;

// Setup scene
var scene = GetScene();

// Render
Console.Error.WriteLine("Rendering...");

if (incremental)
    RenderWithIncrementalRenderer(scene);
else
    RenderWithParallelRenderer(scene);

Console.Error.Write($"Done ({sw.Elapsed})\n");

IScene GetScene()
{
    if (commandLine.Length < 3 || !int.TryParse(commandLine[2], out var sceneIndex))
        sceneIndex = 0;
    
    return sceneIndex switch
    {
        1 => new RandomScene(),
        2 => new TwoSpheres(),
        3 => new TwoPerlinSpheres(),
        4 => new Earth(),
        5 => new SimpleLight(),
        6 => new CornellBox(),
        7 => new CornellSmoke(),
        8 or _ => new FinalScene()
    };
}

Stream GetOutputStream(out IFormat format)
{
    if (commandLine.Length >= 2)
    {
        var targetFileName = Environment.GetCommandLineArgs()[1];

        if (targetFileName.EndsWith(".png"))
            format = new PngFormat();
        else if (targetFileName.EndsWith(".ppm"))
            format = new PpmFormat();
        else
            throw new Exception($"Unsupported file format: {targetFileName}");

        return File.Create(targetFileName);
    }

    format = new PpmFormat();
    return Console.OpenStandardOutput();
}

void RenderWithParallelRenderer(IScene s)
{
    var renderer = new ParallelRenderer(s.GetRenderSettings());
    var pixels = renderer.RenderScene(s.GetBackground(), s.GetWorld(), s.GetCamera(), Console.Error.Write);

    // Output
    Console.Error.Write("Writing file... ");
    using var outputStream = GetOutputStream(out var format);
    format.WriteFile(outputStream, pixels);
}

void RenderWithIncrementalRenderer(IScene s)
{
    var renderer = new IncrementalRenderer(s.GetRenderSettings());
    renderer.RenderScene(
        s.GetBackground(),
        s.GetWorld(),
        s.GetCamera(), 
        WriteFrame,
        Console.Error.Write);

    void WriteFrame(Vec3[,] pixels)
    {
        using var outputStream = GetOutputStream(out var format);
        format.WriteFile(outputStream, pixels);
    }
}