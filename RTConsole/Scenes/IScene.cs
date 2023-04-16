using RTLib;
using RTLib.Hittables;
using RTLib.Model;

namespace RTConsole.Scenes;

public interface IScene
{
    Vec3 GetBackground();
    RenderSettings GetRenderSettings();
    Camera GetCamera();
    IHittable GetWorld();
}