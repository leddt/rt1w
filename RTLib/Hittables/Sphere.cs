using RTLib.Materials;
using RTLib.Model;

namespace RTLib.Hittables;

public class Sphere : IHittable
{
    public Sphere(Vec3 center, double radius, IMaterial material)
    {
        Center = center;
        Radius = radius;
        Material = material;
    }

    public readonly Vec3 Center;
    public readonly double Radius;
    public readonly IMaterial Material;

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
        GetSphereUv(outwardNormal, out rec.U, out rec.V);
        rec.Material = Material;

        return true;
    }

    public bool GetBoundingBox(double time0, double time1, out BoundingBox output)
    {
        output = new BoundingBox(
            Center - new Vec3(Radius, Radius, Radius),
            Center + new Vec3(Radius, Radius, Radius)
        );
        
        return true;
    }

    private static void GetSphereUv(Vec3 p, out double u, out double v)
    {
        // p: a given point on the sphere of radius one, centered at the origin.
        // u: returned value [0,1] of angle around the Y axis from X=-1.
        // v: returned value [0,1] of angle from Y=-1 to Y=+1.
        //     <1 0 0> yields <0.50 0.50>       <-1  0  0> yields <0.00 0.50>
        //     <0 1 0> yields <0.50 1.00>       < 0 -1  0> yields <0.50 0.00>
        //     <0 0 1> yields <0.25 0.50>       < 0  0 -1> yields <0.75 0.50>
        
        var theta = Math.Acos(-p.Y);
        var phi = Math.Atan2(-p.Z, p.X) + Math.PI;

        u = phi / (2 * Math.PI);
        v = theta / Math.PI;
    }
}