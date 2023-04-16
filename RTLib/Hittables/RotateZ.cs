using RTLib.Model;

namespace RTLib.Hittables;

public class RotateZ : IHittable
{
    private readonly IHittable _hittable;
    private readonly double _sinTheta, _cosTheta;
    private bool _hasBox;
    private BoundingBox _bbox;

    public RotateZ(IHittable hittable, double angle)
    {
        _hittable = hittable;

        var radians = Utils.DegreesToRadians(angle);
        _sinTheta = Math.Sin(radians);
        _cosTheta = Math.Cos(radians);
        _hasBox = hittable.GetBoundingBox(0, 1, out _bbox);

        var min = new Vec3(double.PositiveInfinity, double.PositiveInfinity, double.PositiveInfinity);
        var max = new Vec3(double.NegativeInfinity, double.NegativeInfinity, double.NegativeInfinity);
        
        for (var i = 0; i < 2; i++)
        for (var j = 0; j < 2; j++)
        for (var k = 0; k < 2; k++)
        {
            var x = i * _bbox.Maximum.X + (1 - i) * _bbox.Minimum.X;
            var y = j * _bbox.Maximum.Y + (1 - j) * _bbox.Minimum.Y;
            var z = k * _bbox.Maximum.Z + (1 - k) * _bbox.Minimum.Z;

            var newX = _cosTheta * x + _sinTheta * y;
            var newY = -_sinTheta * x + _cosTheta * y;

            var tester = new Vec3(newX, newY, z);

            min = new Vec3(Math.Min(min.X, tester.X), Math.Min(min.Y, tester.Y), Math.Min(min.Z, tester.Z));
            max = new Vec3(Math.Max(max.X, tester.X), Math.Max(max.Y, tester.Y), Math.Max(max.Z, tester.Z));
        }

        _bbox = new BoundingBox(min, max);
    }

    public bool Hit(Ray r, double tMin, double tMax, ref Hit rec)
    {
        var origin = new Vec3(
            _cosTheta * r.Origin.X - _sinTheta * r.Origin.Y,
            _sinTheta * r.Origin.X + _cosTheta * r.Origin.Y,
            r.Origin.Z
        );

        var direction = new Vec3(
            _cosTheta * r.Direction.X - _sinTheta * r.Direction.Y,
            _sinTheta * r.Direction.X + _cosTheta * r.Direction.Y,
            r.Direction.Z
        );

        var rotatedRay = new Ray(origin, direction, r.Time);

        if (!_hittable.Hit(rotatedRay, tMin, tMax, ref rec))
            return false;

        var p = new Vec3(
            _cosTheta * rec.P.X + _sinTheta * rec.P.Y,
            -_sinTheta * rec.P.X + _cosTheta * rec.P.Y,
            rec.P.Z
        );

        var normal = new Vec3(
            _cosTheta * rec.Normal.X + _sinTheta * rec.Normal.Y,
            -_sinTheta * rec.Normal.X + _cosTheta * rec.Normal.Y,
            rec.Normal.Z
        );

        rec.P = p;
        rec.SetFaceNormal(rotatedRay, normal);

        return true;
    }

    public bool GetBoundingBox(double time0, double time1, out BoundingBox output)
    {
        output = _bbox;
        return _hasBox;
    }
}