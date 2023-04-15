using RTLib.Model;
using RTLib.Textures;

namespace RTLib.Materials;

public class Isotropic : IMaterial
{
    private readonly ITexture _albedo;

    public Isotropic(Vec3 color) : this(new SolidColor(color))
    {
    }
    
    public Isotropic(ITexture a)
    {
        _albedo = a;
    }

    public bool Scatter(Ray rIn, Hit rec, out Vec3 attenuation, out Ray scattered)
    {
        scattered = new Ray(rec.P, Vec3.RandomInUnitSphere(), rIn.Time);
        attenuation = _albedo.Value(rec.U, rec.V, rec.P);
        return true;
    }
}