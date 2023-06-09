﻿namespace RTLib;

public readonly struct Vec3
{
    public static readonly Vec3 Zero = new(0, 0, 0);
    
    public Vec3(double x, double y, double z)
    {
        X = x;
        Y = y;
        Z = z;
    }

    public readonly double X;
    public readonly double Y;
    public readonly double Z;

    [Obsolete("Can be pretty slow")]
    public double this[int a]
    {
        get
        {
            return a switch
            {
                0 => X,
                1 => Y,
                2 => Z,
                _ => throw new IndexOutOfRangeException()
            };
        }
    }

    public override string ToString() => $"{X} {Y} {Z}";

    public double Length => Math.Sqrt(LengthSquared);
    public double LengthSquared => X * X + Y * Y + Z * Z;

    public Vec3 ToRgb(int samplesPerPixel)
    {
        // Divide the color by the number of samples and gamma-correct for gamma=2.0.
        var scale = 1.0 / samplesPerPixel;
        return new Vec3(
            Math.Sqrt(X * scale),
            Math.Sqrt(Y * scale),
            Math.Sqrt(Z * scale)
        );
    }

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
    public Vec3 UnitVector() => this / Length;

    public Vec3 Reflect(Vec3 n) => this - 2 * Dot(this, n) * n;

    public Vec3 Refract(Vec3 n, double etaiOverEtat)
    {
        var cosTheta = Math.Min(Dot(-this, n), 1);
        var rOutPerp = etaiOverEtat * (this + cosTheta * n);
        var rOutParallel = -Math.Sqrt(Math.Abs(1 - rOutPerp.LengthSquared)) * n;
        return rOutPerp + rOutParallel;
    }
    
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
        return RandomInUnitSphere().UnitVector();
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
}