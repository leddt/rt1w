using RTLib.Model;

namespace RTLib.Materials;

public class Metal : IMaterial
{
    private readonly Vec3 _albedo;
    private readonly double _fuzz;

    public Metal(Vec3 albedo, double fuzz)
    {
        _albedo = albedo;
        _fuzz = fuzz;
    }

    public bool Scatter(Ray rIn, Hit rec, out Vec3 attenuation, out Ray scattered)
    {
        var reflected = Vec3.Reflect(Vec3.UnitVector(rIn.Direction), rec.Normal);
        scattered = new Ray(rec.P, reflected + _fuzz * Vec3.RandomInUnitSphere(), rIn.Time);
        attenuation = _albedo;
        return Vec3.Dot(scattered.Direction, rec.Normal) > 0;
    }
}