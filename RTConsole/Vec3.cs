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

    public override string ToString() => $"{X} {Y} {Z}";
    
    public double this[int index] => _components[index];

    public static Vec3 operator +(Vec3 a, Vec3 b) => new(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
    public static Vec3 operator -(Vec3 a) => new(-a.X, -a.Y, -a.Z);
    public static Vec3 operator -(Vec3 a, Vec3 b) => new(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
    public static Vec3 operator *(Vec3 vec, double t) => new(vec.X * t, vec.Y * t, vec.Z * t);
    public static Vec3 operator *(double t, Vec3 vec) => vec * t;
    public static Vec3 operator /(Vec3 vec, double t) => vec * (1 / t);
    
    public static double Dot(Vec3 a, Vec3 b) => a.X * b.X + a.Y * b.Y + a.Z * b.Z;
    public static Vec3 Cross(Vec3 a, Vec3 b) => new Vec3(
        a.Y * b.Z - a.Z * b.Y,
        a.Z * b.X - a.X * b.Z,
        a.X * b.Y - a.Y * b.X
    );
    public static Vec3 UnitVector(Vec3 v) => v / v.Length;
}