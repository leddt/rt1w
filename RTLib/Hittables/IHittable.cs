using RTLib.Model;

namespace RTLib.Hittables;

public interface IHittable
{
    bool Hit(Ray r, double tMin, double tMax, ref Hit rec);
    bool GetBoundingBox(double time0, double time1, out BoundingBox output);
}