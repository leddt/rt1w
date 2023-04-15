using RTLib.Materials;
using RTLib.Model;
using RTLib.Textures;

namespace RTLib.Hittables;

public class ConstantMedium : IHittable
{
    private IHittable _boundary;
    private double _negInvDensity;
    private IMaterial _phaseFunction;
    
    public ConstantMedium(IHittable boundary, double d, ITexture texture)
    {
        _boundary = boundary;
        _negInvDensity = -1 / d;
        _phaseFunction = new Isotropic(texture);
    }
    
    public ConstantMedium(IHittable boundary, double d, Vec3 color)
    {
        _boundary = boundary;
        _negInvDensity = -1 / d;
        _phaseFunction = new Isotropic(color);
    }

    public bool Hit(Ray r, double tMin, double tMax, ref Hit rec)
    {
        Hit rec1 = new(), rec2 = new();

        if (!_boundary.Hit(r, double.NegativeInfinity, double.PositiveInfinity, ref rec1)) return false;
        if (!_boundary.Hit(r, rec1.T + 0.0001, double.PositiveInfinity, ref rec2)) return false;

        if (rec1.T < tMin) rec1.T = tMin;
        if (rec2.T > tMax) rec2.T = tMax;

        if (rec1.T >= rec2.T) return false;

        if (rec1.T < 0)
            rec1.T = 0;

        var rayLength = r.Direction.Length;
        var distanceInsideBoundary = (rec2.T - rec1.T) * rayLength;
        var hitDistance = _negInvDensity * Math.Log(Random.Shared.NextDouble());

        if (hitDistance > distanceInsideBoundary)
            return false;

        rec.T = rec1.T + hitDistance / rayLength;
        rec.P = r.At(rec.T);

        rec.Normal = new Vec3(1, 0, 0); // arbitrary
        rec.FrontFace = true; // also arbitrary
        rec.Material = _phaseFunction;

        return true;
    }

    public bool GetBoundingBox(double time0, double time1, out BoundingBox output)
    {
        return _boundary.GetBoundingBox(time0, time1, out output);
    }
}