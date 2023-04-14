namespace RTConsole;

public struct Vec3
{
    private readonly double[] _components = new double[3];
    
    public Vec3() : this(0, 0, 0)
    {
    }

    public Vec3(double x, double y, double z)
    {
        _components = new[] { x, y, z };
    }

    public double X => _components[0];
    public double Y => _components[1];
    public double Z => _components[2];

    public double Length => Math.Sqrt(LengthSquared);
    public double LengthSquared => X * X + Y * Y + Z * Z;
    
    public double this[int index] => _components[index];

    public static Vec3 operator -(Vec3 a)
    {
        return new Vec3(-a.X, -a.Y, -a.Z);
    }
    
    public static Vec3 operator +(Vec3 a, Vec3 b)
    {
        return new Vec3(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
    }

    public static Vec3 operator *(Vec3 vec, double t)
    {
        return new Vec3(vec.X * t, vec.Y * t, vec.Z * t);
    }

    public static Vec3 operator /(Vec3 vec, double t)
    {
        return vec * (1 / t);
    }
}