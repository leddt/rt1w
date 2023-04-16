namespace RTLib;

public class Perlin
{
    private const int PointCount = 256;
    private readonly Vec3[] _ranvec;
    private readonly int[] _permX;
    private readonly int[] _permY;
    private readonly int[] _permZ;

    public Perlin()
    {
        _ranvec = new Vec3[PointCount];
        for (var i = 0; i < PointCount; i++)
            _ranvec[i] = Vec3.Random(-1, 1).UnitVector();

        _permX = GeneratePerm();
        _permY = GeneratePerm();
        _permZ = GeneratePerm();
    }

    public double Noise(Vec3 p)
    {
        var u = p.X - Math.Floor(p.X);
        var v = p.Y - Math.Floor(p.Y);
        var w = p.Z - Math.Floor(p.Z);

        var i = (int)Math.Floor(p.X);
        var j = (int)Math.Floor(p.Y);
        var k = (int)Math.Floor(p.Z);

        var c = new Vec3[2, 2, 2];
        
        for (var di = 0; di < 2; di++)
        for (var dj = 0; dj < 2; dj++)
        for (var dk = 0; dk < 2; dk++)
        {
            c[di, dj, dk] = _ranvec[
                _permX[(i + di) & 255] ^
                _permY[(j + dj) & 255] ^
                _permZ[(k + dk) & 255]
            ];
        }

        return PerlinInterpolation(c, u, v, w);
    }

    public double Turbulence(Vec3 p, int depth = 7)
    {
        var accum = 0.0;
        var tempP = p;
        var weight = 1.0;

        for (var i = 0; i < depth; i++)
        {
            accum += weight * Noise(tempP);
            weight *= 0.5;
            tempP *= 2;
        }

        return Math.Abs(accum);
    }

    private static double TrilinearInterpolation(double[,,] c, double u, double v, double w)
    {
        var accum = 0.0;
        
        for (var i = 0; i<2;i++)
        for (var j = 0; j<2;j++)
        for (var k = 0; k < 2; k++)
        {
            accum += (i * u + (1 - i) * (1 - u)) *
                     (j * v + (1 - j) * (1 - v)) *
                     (k * w + (1 - k) * (1 - w)) *
                     c[i, j, k];
        }

        return accum;
    }

    private static double PerlinInterpolation(Vec3[,,] c, double u, double v, double w)
    {
        var uu = u * u * (3 - 2 * u);
        var vv = v * v * (3 - 2 * v);
        var ww = w * w * (3 - 2 * w);
        var accum = 0.0;
        
        for (var i = 0; i<2;i++)
        for (var j = 0; j<2;j++)
        for (var k = 0; k < 2; k++)
        {
            var weightV = new Vec3(u - i, v - j, w - k);
            accum += (i * uu + (1 - i) * (1 - uu)) *
                     (j * vv + (1 - j) * (1 - vv)) *
                     (k * ww + (1 - k) * (1 - ww)) *
                     Vec3.Dot(c[i, j, k], weightV);
        }

        return accum;
    }

    private static int[] GeneratePerm()
    {
        var p = new int[PointCount];
        
        for (var i = 0; i < PointCount; i++)
            p[i] = i;

        Permute(p, PointCount);

        return p;
    }

    private static void Permute(int[] p, int n)
    {
        for (var i = n - 1; i > 0; i--)
        {
            var target = Random.Shared.Next(0, i + 1);
            (p[i], p[target]) = (p[target], p[i]);
        }
    }
}