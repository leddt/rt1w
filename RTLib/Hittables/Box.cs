using RTLib.Materials;
using RTLib.Model;

namespace RTLib.Hittables;

public class Box : IHittable
{
    private readonly Vec3 _boxMin;
    private readonly Vec3 _boxMax;
    private readonly IMaterial _material;

    private readonly HittableList _sides = new();
    
    public Box(Vec3 boxMin, Vec3 boxMax, IMaterial material)
    {
        _boxMin = boxMin;
        _boxMax = boxMax;
        _material = material;

        _sides.Add(new RectXY(boxMin.X, boxMax.X, boxMin.Y, boxMax.Y, boxMax.Z, material));
        _sides.Add(new RectXY(boxMin.X, boxMax.X, boxMin.Y, boxMax.Y, boxMin.Z, material));
        
        _sides.Add(new RectXZ(boxMin.X, boxMax.X, boxMin.Z, boxMax.Z, boxMax.Y, material));
        _sides.Add(new RectXZ(boxMin.X, boxMax.X, boxMin.Z, boxMax.Z, boxMin.Y, material));
        
        _sides.Add(new RectYZ(boxMin.Y, boxMax.Y, boxMin.Z, boxMax.Z, boxMax.X, material));
        _sides.Add(new RectYZ(boxMin.Y, boxMax.Y, boxMin.Z, boxMax.Z, boxMin.X, material));
    }

    public bool Hit(Ray r, double tMin, double tMax, ref Hit rec)
    {
        return _sides.Hit(r, tMin, tMax, ref rec);
    }

    public bool GetBoundingBox(double time0, double time1, out BoundingBox output)
    {
        output = new BoundingBox(_boxMin, _boxMax);
        return true;
    }
}