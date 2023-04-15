using RTLib.Model;

namespace RTLib.Hittables;

public class BoundingBox
{
    public readonly Vec3 Minimum;
    public readonly Vec3 Maximum;

    public BoundingBox() { }

    public BoundingBox(Vec3 a, Vec3 b)
    {
        Minimum = a;
        Maximum = b;
    }

    public bool Hit(Ray r, double tMin, double tMax)
    {
        for (var a = 0; a < 3; a++)
        {
            // Original
            // var t0 = Math.Min(
            //     (Minimum[a] - r.Origin[a]) / r.Direction[a],
            //     (Maximum[a] - r.Origin[a]) / r.Direction[a]
            // );
            // var t1 = Math.Max(
            //     (Minimum[a] - r.Origin[a]) / r.Direction[a],
            //     (Maximum[a] - r.Origin[a]) / r.Direction[a]
            // );
            // tMin = Math.Max(t0, tMin);
            // tMax = Math.Min(t1, tMax);
            
            // Optimized
            var invD = 1.0f / r.Direction[a];
            var t0 = (Minimum[a] - r.Origin[a]) * invD;
            var t1 = (Maximum[a] - r.Origin[a]) * invD;
            if (invD < 0.0f)
                (t0, t1) = (t1, t0);
            tMin = t0 > tMin ? t0 : tMin;
            tMax = t1 < tMax ? t1 : tMax;

            if (tMax <= tMin) return false;
        }

        return true;
    }

    public static BoundingBox Surrounding(BoundingBox box0, BoundingBox box1)
    {
        var small = new Vec3(
            Math.Min(box0.Minimum.X, box1.Minimum.X),
            Math.Min(box0.Minimum.Y, box1.Minimum.Y),
            Math.Min(box0.Minimum.Z, box1.Minimum.Z)
        );
        var big = new Vec3(
            Math.Max(box0.Maximum.X, box1.Maximum.X),
            Math.Max(box0.Maximum.Y, box1.Maximum.Y),
            Math.Max(box0.Maximum.Z, box1.Maximum.Z)
        );

        return new BoundingBox(small, big);
    }
}