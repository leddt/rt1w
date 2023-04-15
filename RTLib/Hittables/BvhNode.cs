using RTLib.Model;

namespace RTLib.Hittables;

public class BvhNode : IHittable
{
    public readonly BoundingBox Box;
    public readonly IHittable Left;
    public readonly IHittable Right;
    
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
                Left = Right = objects[0];
                break;
            case 2 when comparator(objects[0], objects[1]) > 0:
                Left = objects[0];
                Right = objects[1];
                break;
            case 2:
                Left = objects[1];
                Right = objects[0];
                break;
            default:
            {
                objects.Sort(comparator);
                var mid = objects.Length / 2;
                Left = new BvhNode(objects.Slice(0, mid), time0, time1);
                Right = new BvhNode(objects.Slice(mid), time0, time1);
                break;
            }
        }

        if (!Left.GetBoundingBox(time0, time1, out var boxLeft) ||
            !Right.GetBoundingBox(time0, time1, out var boxRight))
            throw new Exception("No bounding box in BvhNode constructor.");

        Box = BoundingBox.Surrounding(boxLeft, boxRight);
    }

    public bool Hit(Ray r, double tMin, double tMax, ref Hit rec)
    {
        if (!Box.Hit(r, tMin, tMax)) return false;

        var hitLeft = Left.Hit(r, tMin, tMax, ref rec);
        var hitRight = Right.Hit(r, tMin, hitLeft ? rec.T : tMax, ref rec);

        return hitLeft || hitRight;
    }

    public bool GetBoundingBox(double time0, double time1, out BoundingBox output)
    {
        output = Box;
        return true;
    }

    private int CompareX(IHittable a, IHittable b) => Compare(a, b, 0);
    private int CompareY(IHittable a, IHittable b) => Compare(a, b, 1);
    private int CompareZ(IHittable a, IHittable b) => Compare(a, b, 2);

    private int Compare(IHittable a, IHittable b, int axis)
    {
        if (!a.GetBoundingBox(0, 0, out var boxA) ||
            !b.GetBoundingBox(0, 0, out var boxB))
            throw new Exception("No bounding box in BvhNode comparison.");

        return boxA.Minimum[axis].CompareTo(boxB.Minimum[axis]);
    }
}