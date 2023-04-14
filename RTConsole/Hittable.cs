namespace RTConsole;

public struct Hit
{
    public Vec3 P;
    public Vec3 Normal;
    public double T;
}

public abstract class Hittable
{
    public virtual bool Hit(Ray r, double tMin, double tMax, ref Hit rec) => false;
}