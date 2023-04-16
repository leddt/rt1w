using RTLib.Materials;
using RTLib.Model;

namespace RTLib.Hittables;

public class RectXZ : IHittable
{
    private readonly double _x0, _x1, _z0, _z1, _k;
    private readonly IMaterial _material;
    private readonly bool _singleFace;
    private readonly bool _flip;

    public RectXZ(double x0, double x1, double z0, double z1, double k, IMaterial material, bool singleFace = false, bool flip = false)
    {
        _x0 = x0;
        _x1 = x1;
        _z0 = z0;
        _z1 = z1;
        _k = k;
        _material = material;
        _singleFace = singleFace;
        _flip = flip;
    }

    public bool Hit(Ray r, double tMin, double tMax, ref Hit rec)
    {
        var t = (_k - r.Origin.Y) / r.Direction.Y;
        if (t < tMin || t > tMax)
            return false;

        var x = r.Origin.X + t * r.Direction.X;
        var z = r.Origin.Z + t * r.Direction.Z;
        if (x < _x0 || x > _x1 || z < _z0 || z > _z1)
            return false;

        rec.U = (x - _x0) / (_x1 - _x0);
        rec.V = (z - _z0) / (_z1 - _z0);
        rec.T = t;

        var outwardNormal = new Vec3(0, 1, 0);
        rec.SetFaceNormal(r, outwardNormal);
        
        if (_singleFace && rec.FrontFace == _flip)
            return false;
        
        rec.Material = _material;
        rec.P = r.At(t);
        return true;
    }

    public bool GetBoundingBox(double time0, double time1, out BoundingBox output)
    {
        // The bounding box must have non-zero width in each dimension, so pad the Y dimension a small amount.
        output = new BoundingBox(new Vec3(_x0, _k - 0.0001, _z0), new Vec3(_x1, _k + 0.0001, _z1));
        return true;
    }
}