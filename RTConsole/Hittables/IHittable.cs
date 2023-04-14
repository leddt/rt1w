namespace RTConsole.Hittables;

public interface IHittable
{
    bool Hit(Ray r, double tMin, double tMax, ref Hit rec);
}