using RTLib.Model;
using RTLib.Textures;

namespace RTLib.Materials;

public class Lambertian : IMaterial
{
    private readonly ITexture _albedo;

    public Lambertian(Vec3 albedo)
    {
        _albedo = new SolidColor(albedo);
    }

    public Lambertian(ITexture albedo)
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
        attenuation = _albedo.Value(rec.U, rec.V, rec.P);
        return true;
    }
}