using RTLib;
using RTLib.Hittables;
using RTLib.Model;

namespace RTConsole.Scenes;

public abstract class BaseScene : IScene 
{
    public virtual Vec3 GetBackground() => new(0, 0, 0);
    
    public RenderSettings GetRenderSettings() => new(AspectRatio, ImageWidth, SamplesPerPixel, MaxDepth);

    public Camera GetCamera() => new(
        LookFrom, LookAt,
        CameraUp,
        VerticalFov,
        AspectRatio,
        Aperture,
        FocusDistance,
        TimeStart, TimeEnd
    );

    public IHittable GetWorld()
    {
        var list = new HittableList();
        foreach (var obj in GetSceneObjects()) list.Add(obj);
        return list;
    }

    protected virtual double AspectRatio => 16.0 / 9.0;
    protected virtual int SamplesPerPixel => 500;
    protected virtual int ImageWidth => 600;
    protected virtual int MaxDepth => 50;
    
    protected abstract Vec3 LookFrom { get; }
    protected abstract Vec3 LookAt { get; }
    protected virtual Vec3 CameraUp => new(0, 1, 0);
    protected virtual double VerticalFov => 40;
    protected virtual double Aperture => 0;
    protected virtual double FocusDistance => (LookAt - LookFrom).Length;
    protected virtual double TimeStart => 0;
    protected virtual double TimeEnd => 1;
    
    protected abstract IEnumerable<IHittable> GetSceneObjects();
}