namespace RTLib;

public class Perlin
{
    private const int PointCount = 256;
    private readonly double[] _ranfloat;
    private readonly int[] _permX;
    private readonly int[] _permY;
    private readonly int[] _permZ;

    public Perlin()
    {
        _ranfloat = new double[PointCount];
        for (var i = 0; i < PointCount; i++)
            _ranfloat[i] = Random.Shared.NextDouble();

        _permX = GeneratePerm();
        _permY = GeneratePerm();
        _permZ = GeneratePerm();
    }

    public double Noise(Vec3 p)
    {
        var i = (int)(4 * p.X) & 255;
        var j = (int)(4 * p.Y) & 255;
        var k = (int)(4 * p.Z) & 255;

        return _ranfloat[_permX[i] ^ _permY[j] ^ _permZ[k]];
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