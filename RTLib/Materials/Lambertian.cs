using RTLib.Model;

namespace RTLib.Materials;

public class Lambertian : IMaterial
{
    private readonly Vec3 _albedo;

    public Lambertian(Vec3 albedo)
    {
        _albedo = albedo;
    }

    public bool Scatter(Ray rIn, Hit rec, out Vec3 attenuation, out Ray scattered)
    {
        // var scatterDirection = Vec3.RandomInHemisphere(rec.Normal);
        var scatterDirection = rec.Normal + Vec3.RandomUnitVector();
        
        // Catch degenerate scatter direction
        if (scatterDirection.NearZero)
            scatterDirection = rec.Normal;
        
        scattered = new Ray(rec.P, scatterDirection, rIn.Time);
        attenuation = _albedo;
        return true;
    }
}