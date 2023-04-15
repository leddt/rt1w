using RTLib.Hittables;

namespace RTLib;

public static class Utils
{
    public static double DegreesToRadians(double degrees) => degrees * Math.PI / 180;
    public static double NextDouble(this Random rng, double min, double max) => min + (max - min) * rng.NextDouble();

    public static IHittable RotateY(this IHittable hittable, double angle) => new RotateY(hittable, angle);
    public static IHittable Translate(this IHittable hittable, Vec3 offset) => new Translate(hittable, offset);
    public static IHittable Translate(this IHittable hittable, double x, double y, double z) =>
        new Translate(hittable, new Vec3(x, y, z));
}