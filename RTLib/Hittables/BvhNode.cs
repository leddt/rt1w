using RTLib.Model;

namespace RTLib.Hittables;

public class BvhNode : IHittable
{
    private readonly BoundingBox _box;
    private readonly IHittable _left;
    private readonly IHittable _right;
    
    public BvhNode(HittableList list, double time0, double time1) : this(list.Objects, time0, time1)
    {
    }
    
    public BvhNode(Span<IHittable> objects, double time0, double time1)
    {
        var axis = Random.Shared.Next(0, 3);
        Comparison<IHittable> comparator = axis switch
        {
            0 => CompareX,
            1 => CompareY,
            _ => CompareZ
        };

        switch (objects.Length)
        {
            case 1:
                _left = _right = objects[0];
                break;
            case 2 when comparator(objects[0], objects[1]) > 0:
                _left = objects[0];
                _right = objects[1];
                break;
            case 2:
                _left = objects[1];
                _right = objects[0];
                break;
            default:
            {
                objects.Sort(comparator);
                var mid = objects.Length / 2;
                _left = new BvhNode(objects.Slice(0, mid), time0, time1);
                _right = new BvhNode(objects.Slice(mid), time0, time1);
                break;
            }
        }

        if (!_left.GetBoundingBox(time0, time1, out var boxLeft) ||
            !_right.GetBoundingBox(time0, time1, out var boxRight))
            throw new Exception("No bounding box in BvhNode constructor.");

        _box = BoundingBox.Surrounding(boxLeft, boxRight);
    }

    public bool Hit(Ray r, double tMin, double tMax, ref Hit rec)
    {
        if (!_box.Hit(r, tMin, tMax)) return false;

        var hitLeft = _left.Hit(r, tMin, tMax, ref rec);
        var hitRight = _right.Hit(r, tMin, hitLeft ? rec.T : tMax, ref rec);

        return hitLeft || hitRight;
    }

    public bool GetBoundingBox(double time0, double time1, out BoundingBox output)
    {
        output = _box;
        return true;
    }

    private int CompareX(IHittable a, IHittable b)
    {
        if (!a.GetBoundingBox(0, 0, out var boxA) ||
            !b.GetBoundingBox(0, 0, out var boxB))
            throw new Exception("No bounding box in BvhNode comparison.");

        return boxA.Minimum.X.CompareTo(boxB.Minimum.X);
    }

    private int CompareY(IHittable a, IHittable b)
    {
        if (!a.GetBoundingBox(0, 0, out var boxA) ||
            !b.GetBoundingBox(0, 0, out var boxB))
            throw new Exception("No bounding box in BvhNode comparison.");

        return boxA.Minimum.Y.CompareTo(boxB.Minimum.Y);
    }

    private int CompareZ(IHittable a, IHittable b)
    {
        if (!a.GetBoundingBox(0, 0, out var boxA) ||
            !b.GetBoundingBox(0, 0, out var boxB))
            throw new Exception("No bounding box in BvhNode comparison.");

        return boxA.Minimum.Z.CompareTo(boxB.Minimum.Z);
    }
}