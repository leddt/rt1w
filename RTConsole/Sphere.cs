﻿namespace RTConsole;

public class Sphere : IHittable
{
    public Sphere(Vec3 center, double radius)
    {
        Center = center;
        Radius = radius;
    }

    public Vec3 Center { get; }
    public double Radius { get; }

    public bool Hit(Ray r, double tMin, double tMax, ref Hit rec)
    {
        var oc = r.Origin - Center;
        var a = r.Direction.LengthSquared;
        var halfB = Vec3.Dot(oc, r.Direction);
        var c = oc.LengthSquared - Radius * Radius;

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
        var outwardNormal = (rec.P - Center) / Radius;
        rec.SetFaceNormal(r, outwardNormal);

        return true;
    }
}