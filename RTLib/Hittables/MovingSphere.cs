using RTLib.Materials;
using RTLib.Model;

namespace RTLib.Hittables;

public class MovingSphere : IHittable
{
    private readonly Vec3 _center0, _center1;
    private readonly double _time0, _time1;
    private readonly double _radius;
    private readonly IMaterial _material;
    
    public MovingSphere(Vec3 center0, Vec3 center1, double time0, double time1, double r, IMaterial material)
    {
        _center0 = center0;
        _center1 = center1;
        _time0 = time0;
        _time1 = time1;
        _radius = r;
        _material = material;
    }

    public bool Hit(Ray r, double tMin, double tMax, ref Hit rec)
    {
        var oc = r.Origin - GetCenter(r.Time);
        var a = r.Direction.LengthSquared;
        var halfB = Vec3.Dot(oc, r.Direction);
        var c = oc.LengthSquared - _radius * _radius;

        var discriminant = halfB * halfB - a * c;
        if (discriminant < 0) return false;
        var sqrtD = Math.Sqrt(discriminant);

        // Find the nearest root that lies in the acceptable range.
        var root = (-halfB - sqrtD) / a;
        if (root < tMin || tMax < root)
        {
            root = (-halfB - sqrtD) / a;
            if (root < tMin || tMax < root)
                return false;
        }

        rec.T = root;
        rec.P = r.At(rec.T);
        var outwardNormal = (rec.P - GetCenter(r.Time)) / _radius;
        rec.SetFaceNormal(r, outwardNormal);
        rec.Material = _material;

        return true;
    }

    public bool GetBoundingBox(double time0, double time1, out BoundingBox output)
    {
        var box0 = new BoundingBox(
            GetCenter(time0) - new Vec3(_radius, _radius, _radius),
            GetCenter(time0) + new Vec3(_radius, _radius, _radius)
        );
        var box1 = new BoundingBox(
            GetCenter(time1) - new Vec3(_radius, _radius, _radius),
            GetCenter(time1) + new Vec3(_radius, _radius, _radius)
        );

        output = BoundingBox.Surrounding(box0, box1);
        return true;
    }

    private Vec3 GetCenter(double time)
    {
        return _center0 + (time - _time0) / (_time1 - _time0) * (_center1 - _center0);
    }
}