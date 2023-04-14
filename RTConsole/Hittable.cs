namespace RTConsole;

public struct Hit
{
    public Vec3 P;
    public Vec3 Normal;
    public double T;
    public bool FrontFace;

    public void SetFaceNormal(Ray r, Vec3 outwardNormal)
    {
        FrontFace = Vec3.Dot(r.Direction, outwardNormal) < 0;
        Normal = FrontFace ? outwardNormal : -outwardNormal;
    }
}

public abstract class Hittable
{
    public virtual bool Hit(Ray r, double tMin, double tMax, ref Hit rec) => false;
}

public class HittableList : Hittable
{
    private readonly ICollection<Hittable> _objects;

    public HittableList(ICollection<Hittable> objects)
    {
        _objects = objects;
    }

    public override bool Hit(Ray r, double tMin, double tMax, ref Hit rec)
    {
        var tempRec = new Hit();
        var hitAnything = false;
        var closestSoFar = tMax;

        foreach (var obj in _objects)
        {
            if (obj.Hit(r, tMin, closestSoFar, ref tempRec))
            {
                hitAnything = true;
                closestSoFar = tempRec.T;
                rec = tempRec;
            }
        }

        return hitAnything;
    }
}