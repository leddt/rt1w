using RTLib.Model;

namespace RTLib.Hittables;

public struct BoundingBox
{
    private readonly Vec3 _minimum;
    private readonly Vec3 _maximum;
    
    public BoundingBox(Vec3 a, Vec3 b)
    {
        _minimum = a;
        _maximum = b;
    }

    public bool Hit(Ray r, double tMin, double tMax)
    {
        for (var a = 0; a < 3; a++)
        {
            // Original
            // var t0 = Math.Min(
            //     (_minimum[a] - r.Origin[a]) / r.Direction[a],
            //     (_maximum[a] - r.Origin[a]) / r.Direction[a]
            // );
            // var t1 = Math.Max(
            //     (_minimum[a] - r.Origin[a]) / r.Direction[a],
            //     (_maximum[a] - r.Origin[a]) / r.Direction[a]
            // );
            //
            // tMin = Math.Max(t0, tMin);
            // tMax = Math.Min(t1, tMax);
            
            // Optimized
            var invD = 1.0 / r.Direction[a];
            var t0 = (_minimum[a] - r.Origin[a]) * invD;
            var t1 = (_maximum[a] - r.Origin[a]) * invD;
            if (invD < 0.0)
                (t0, t1) = (t1, t0);
            
            tMin = t0 > tMin ? t0 : tMin;
            tMax = t1 < tMax ? t1 : tMax;

            if (tMax < tMin) return false;
        }

        return true;
    }

    public static BoundingBox Surrounding(BoundingBox box0, BoundingBox box1)
    {
        var small = new Vec3(
            Math.Min(box0._minimum.X, box1._minimum.X),
            Math.Min(box0._minimum.Y, box1._minimum.Y),
            Math.Min(box0._minimum.Z, box1._minimum.Z)
        );
        var big = new Vec3(
            Math.Max(box0._minimum.X, box1._minimum.X),
            Math.Max(box0._minimum.Y, box1._minimum.Y),
            Math.Max(box0._minimum.Z, box1._minimum.Z)
        );

        return new BoundingBox(small, big);
    }
}