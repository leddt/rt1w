using RTLib.Model;
using RTLib.Textures;

namespace RTLib.Materials;

public class Metal : IMaterial
{
    private readonly ITexture _albedo;
    private readonly double _fuzz;

    public Metal(Vec3 albedo, double fuzz) : this (new SolidColor(albedo), fuzz)
    {
    }
    public Metal(ITexture albedo, double fuzz)
    {
        _albedo = albedo;
        _fuzz = fuzz;
    }

    public bool Scatter(Ray rIn, Hit rec, out Vec3 attenuation, out Ray scattered)
    {
        var reflected = rIn.Direction.UnitVector().Reflect(rec.Normal);
        scattered = new Ray(rec.P, reflected + _fuzz * Vec3.RandomInUnitSphere(), rIn.Time);
        attenuation = _albedo.Value(rec.U, rec.V, rec.P);
        return Vec3.Dot(scattered.Direction, rec.Normal) > 0;
    }
}