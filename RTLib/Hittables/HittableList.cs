using System.Runtime.InteropServices;
using RTLib.Model;

namespace RTLib.Hittables;

public class HittableList : IHittable
{
    private int _count;
    private List<IHittable> _hittables = new();
    public Span<IHittable> Objects => CollectionsMarshal.AsSpan(_hittables);

    public void Add(IHittable hittable)
    {
        _hittables.Add(hittable);
        _count++;
    }
    
    public bool Hit(Ray r, double tMin, double tMax, ref Hit rec)
    {
        var tempRec = new Hit();
        var hitAnything = false;
        var closestSoFar = tMax;

        for (var i = 0; i < _count; i++)
        {
            if (!_hittables[i].Hit(r, tMin, closestSoFar, ref tempRec)) continue;
            
            hitAnything = true;
            closestSoFar = tempRec.T;
            rec = tempRec;
        }

        return hitAnything;
    }

    public bool GetBoundingBox(double time0, double time1, out BoundingBox output)
    {
        output = new BoundingBox();
        
        if (_count == 0)
            return false;

        var firstBox = true;
        for (var i = 0; i < _count; i++)
        {
            if (!_hittables[i].GetBoundingBox(time0, time1, out var temp)) return false;
            output = firstBox ? temp : BoundingBox.Surrounding(output, temp);
            firstBox = false;
        }

        return true;
    }
}