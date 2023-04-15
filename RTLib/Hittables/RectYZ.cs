using RTLib.Materials;
using RTLib.Model;

namespace RTLib.Hittables;

public class RectYZ : IHittable
{
    private readonly double _y0, _y1, _z0, _z1, _k;
    private readonly IMaterial _material;

    public RectYZ(double y0, double y1, double z0, double z1, double k, IMaterial material)
    {
        _y0 = y0;
        _y1 = y1;
        _z0 = z0;
        _z1 = z1;
        _k = k;
        _material = material;
    }

    public bool Hit(Ray r, double tMin, double tMax, ref Hit rec)
    {
        var t = (_k - r.Origin.X) / r.Direction.X;
        if (t < tMin || t > tMax)
            return false;

        var y = r.Origin.Y + t * r.Direction.Y;
        var z = r.Origin.Z + t * r.Direction.Z;
        if (y < _y0 || y > _y1 || z < _z0 || z > _z1)
            return false;

        rec.U = (y - _y0) / (_y1 - _y0);
        rec.V = (z - _z0) / (_z1 - _z0);
        rec.T = t;

        var outwardNormal = new Vec3(1, 0, 0);
        rec.SetFaceNormal(r, outwardNormal);
        rec.Material = _material;
        rec.P = r.At(t);
        return true;
    }

    public bool GetBoundingBox(double time0, double time1, out BoundingBox output)
    {
        // The bounding box must have non-zero width in each dimension, so pad the X dimension a small amount.
        output = new BoundingBox(new Vec3(_k - 0.0001, _y0, _z0), new Vec3(_k + 0.0001, _y1, _z1));
        return true;
    }
}