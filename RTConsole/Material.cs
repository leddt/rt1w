namespace RTConsole;

public abstract class Material
{
    public abstract bool Scatter(Ray rIn, Hit rec, out Vec3 attenuation, out Ray scattered);
}

public class Lambertian : Material
{
    private readonly Vec3 _albedo;

    public Lambertian(Vec3 albedo)
    {
        _albedo = albedo;
    }

    public override bool Scatter(Ray rIn, Hit rec, out Vec3 attenuation, out Ray scattered)
    {
        // var scatterDirection = Vec3.RandomInHemisphere(rec.Normal);
        var scatterDirection = rec.Normal + Vec3.RandomUnitVector();
        
        // Catch degenerate scatter direction
        if (scatterDirection.NearZero)
            scatterDirection = rec.Normal;
        
        scattered = new Ray(rec.P, scatterDirection);
        attenuation = _albedo;
        return true;
    }
}

public class Metal : Material
{
    private readonly Vec3 _albedo;
    private readonly double _fuzz;

    public Metal(Vec3 albedo, double fuzz)
    {
        _albedo = albedo;
        _fuzz = fuzz;
    }

    public override bool Scatter(Ray rIn, Hit rec, out Vec3 attenuation, out Ray scattered)
    {
        var reflected = Vec3.Reflect(Vec3.UnitVector(rIn.Direction), rec.Normal);
        scattered = new Ray(rec.P, reflected + _fuzz * Vec3.RandomInUnitSphere());
        attenuation = _albedo;
        return Vec3.Dot(scattered.Direction, rec.Normal) > 0;
    }
}

public class Dielectric : Material
{
    private readonly double _ir;

    public Dielectric(double indexOfRefraction)
    {
        _ir = indexOfRefraction;
    }

    public override bool Scatter(Ray rIn, Hit rec, out Vec3 attenuation, out Ray scattered)
    {
        attenuation = new Vec3(1, 1, 1);
        var refractionRatio = rec.FrontFace ? 1.0 / _ir : _ir;

        var unitDirection = Vec3.UnitVector(rIn.Direction);
        var refracted = Vec3.Refract(unitDirection, rec.Normal, refractionRatio);

        scattered = new Ray(rec.P, refracted);
        return true;
    }
}