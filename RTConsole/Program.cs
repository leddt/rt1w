using System.Diagnostics;
using RTConsole.Scenes;
using RTLib;
using RTLib.Formats;

var sw = Stopwatch.StartNew();
var commandLine = Environment.GetCommandLineArgs();

// Setup scene
var scene = GetScene();

// Render
Console.Error.WriteLine("Rendering...");
var renderer = new ParallelRenderer(scene.GetRenderSettings());
var pixels = renderer.RenderScene(scene.GetBackground(), scene.GetWorld(), scene.GetCamera(), Console.Error.Write);

// Output
Console.Error.Write("Writing file... ");
using (var outputStream = GetOutputStream(out var format))
{
    format.WriteFile(outputStream, pixels);
}

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