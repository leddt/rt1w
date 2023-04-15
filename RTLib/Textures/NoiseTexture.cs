namespace RTLib.Textures;

public class NoiseTexture : ITexture
{
    private readonly Perlin _perlin;
    private readonly double _scale;

    public NoiseTexture(double scale)
    {
        _perlin = new Perlin();
        _scale = scale;
    }

    public Vec3 Value(double u, double v, Vec3 p)
    {
        return new Vec3(1, 1, 1) * _perlin.Noise(_scale * p);
    }
}