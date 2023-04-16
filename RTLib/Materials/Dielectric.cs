using RTLib.Model;

namespace RTLib.Materials;

public class Dielectric : IMaterial
{
    private readonly double _ir;

    public Dielectric(double indexOfRefraction)
    {
        _ir = indexOfRefraction;
    }

    public bool Scatter(Ray rIn, Hit rec, out Vec3 attenuation, out Ray scattered)
    {
        attenuation = new Vec3(1, 1, 1);
        var refractionRatio = rec.FrontFace ? 1.0 / _ir : _ir;

        var unitDirection = rIn.Direction.UnitVector();
        var cosTheta = Math.Min(Vec3.Dot(-unitDirection, rec.Normal), 1.0);
        var sinTheta = Math.Sqrt(1 - cosTheta * cosTheta);

        var cannotRefract = refractionRatio * sinTheta > 1;

        Vec3 direction;
        if (cannotRefract || Reflectance(cosTheta, refractionRatio) > Random.Shared.NextDouble())
            direction = unitDirection.Reflect(rec.Normal);
        else
            direction = unitDirection.Refract(rec.Normal, refractionRatio);

        scattered = new Ray(rec.P, direction, rIn.Time);
        return true;
    }

    private static double Reflectance(double cosine, double refIdx)
    {
        var r0 = (1 - refIdx) / (1 + refIdx);
        r0 *= r0;
        return r0 + (1 - r0) * Math.Pow(1 - cosine, 5);
    }
}