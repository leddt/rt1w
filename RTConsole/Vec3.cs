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

    public void WriteColor(TextWriter writer, int samplesPerPixel)
    {
        // Divide the color by the number of samples and gamma-correct for gamma=2.0.
        var scale = 1.0 / samplesPerPixel;
        var r = Math.Sqrt(X * scale);
        var g = Math.Sqrt(Y * scale);
        var b = Math.Sqrt(Z * scale);

        writer.Write($"{(int)(256 * Math.Clamp(r, 0, 0.999))} " +
                     $"{(int)(256 * Math.Clamp(g, 0, 0.999))} " +
                     $"{(int)(256 * Math.Clamp(b, 0, 0.999))}\n");
    }

    public double this[int index] => _components[index];

    /// <summary>
    /// Return true if the vector is close to zero in all dimensions.
    /// </summary>
    public bool NearZero
    {
        get
        {
            var s = 1e-8;
            return Math.Abs(X) < s &&
                   Math.Abs(Y) < s &&
                   Math.Abs(Z) < s;
        }
    }

    public static Vec3 operator +(Vec3 a, Vec3 b) => new(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
    public static Vec3 operator -(Vec3 a) => new(-a.X, -a.Y, -a.Z);
    public static Vec3 operator -(Vec3 a, Vec3 b) => new(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
    public static Vec3 operator *(Vec3 a, Vec3 b) => new(a.X * b.X, a.Y * b.Y, a.Z * b.Z);
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

    public static Vec3 Random() => new(
        System.Random.Shared.NextDouble(),
        System.Random.Shared.NextDouble(),
        System.Random.Shared.NextDouble()
    );
    
    public static Vec3 Random(double min, double max) => new(
        System.Random.Shared.NextDouble(min, max),
        System.Random.Shared.NextDouble(min, max),
        System.Random.Shared.NextDouble(min, max)
    );

    public static Vec3 RandomInUnitSphere()
    {
        while (true)
        {
            var p = Vec3.Random(-1, 1);
            if (p.LengthSquared >= 1) continue;
            return p;
        }
    }

    public static Vec3 RandomUnitVector()
    {
        return UnitVector(RandomInUnitSphere());
    }

    public static Vec3 RandomInHemisphere(Vec3 normal)
    {
        var inUnitSphere = RandomInUnitSphere();
        if (Dot(inUnitSphere, normal) > 0) // In the same hemisphere as the normal
            return inUnitSphere;
        else
            return -inUnitSphere;
    }

    public static Vec3 RandomInUnitDisk()
    {
        while (true)
        {
            var p = new Vec3(System.Random.Shared.NextDouble(-1, 1), System.Random.Shared.NextDouble(-1, 1), 0);
            if (p.LengthSquared >= 1) continue;
            return p;
        }
    }

    public static Vec3 Reflect(Vec3 v, Vec3 n)
    {
        return v - 2 * Dot(v, n) * n;
    }

    public static Vec3 Refract(Vec3 uv, Vec3 n, double etaiOverEtat)
    {
        var cosTheta = Math.Min(Dot(-uv, n), 1);
        var rOutPerp = etaiOverEtat * (uv + cosTheta * n);
        var rOutParallel = -Math.Sqrt(Math.Abs(1 - rOutPerp.LengthSquared)) * n;
        return rOutPerp + rOutParallel;
    }
}