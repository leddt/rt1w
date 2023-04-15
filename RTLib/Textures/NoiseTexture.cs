namespace RTLib.Textures;

public class NoiseTexture : ITexture
{
    private readonly Perlin _perlin;

    public NoiseTexture()
    {
        _perlin = new Perlin();
    }

    public Vec3 Value(double u, double v, Vec3 p)
    {
        return new Vec3(1, 1, 1) * _perlin.Noise(p);
    }
}