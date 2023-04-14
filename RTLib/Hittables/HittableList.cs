namespace RTLib.Hittables;

public class HittableList : IHittable
{
    private int _count;
    private List<IHittable> _hittables = new();

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
}