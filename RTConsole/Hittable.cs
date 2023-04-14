namespace RTConsole;

public struct Hit
{
    public Vec3 P;
    public Vec3 Normal;
    public Material Material;
    public double T;
    public bool FrontFace;

    public void SetFaceNormal(Ray r, Vec3 outwardNormal)
    {
        FrontFace = Vec3.Dot(r.Direction, outwardNormal) < 0;
        Normal = FrontFace ? outwardNormal : -outwardNormal;
    }
}

public interface IHittable
{
    bool Hit(Ray r, double tMin, double tMax, ref Hit rec);
}

public class HittableList : List<IHittable>, IHittable
{
    public bool Hit(Ray r, double tMin, double tMax, ref Hit rec)
    {
        var tempRec = new Hit();
        var hitAnything = false;
        var closestSoFar = tMax;

        foreach (var obj in this)
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