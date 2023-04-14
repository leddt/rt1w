using RTLib.Model;

namespace RTLib.Hittables;

public interface IHittable
{
    bool Hit(Ray r, double tMin, double tMax, ref Hit rec);
}