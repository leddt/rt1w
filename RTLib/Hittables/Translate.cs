using RTLib.Model;

namespace RTLib.Hittables;

public class Translate : IHittable
{
    private readonly IHittable _hittable;
    private readonly Vec3 _displacement;

    public Translate(IHittable hittable, Vec3 displacement)
    {
        _hittable = hittable;
        _displacement = displacement;
    }

    public bool Hit(Ray r, double tMin, double tMax, ref Hit rec)
    {
        var movedRay = new Ray(r.Origin - _displacement, r.Direction, r.Time);
        if (!_hittable.Hit(movedRay, tMin, tMax, ref rec))
            return false;

        rec.P += _displacement;
        rec.SetFaceNormal(movedRay, rec.Normal);

        return true;
    }

    public bool GetBoundingBox(double time0, double time1, out BoundingBox output)
    {
        if (!_hittable.GetBoundingBox(time0, time1, out output))
            return false;

        output = new BoundingBox(
            output.Minimum + _displacement,
            output.Maximum + _displacement
        );

        return true;
    }
}