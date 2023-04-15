namespace RTLib.Textures;

public class SolidColor : ITexture
{
    private readonly Vec3 _color;

    public SolidColor(Vec3 color)
    {
        _color = color;
    }

    public SolidColor(double red, double green, double blue) : this(new Vec3(red, green, blue))
    {
    }

    public Vec3 Value(double u, double v, Vec3 p)
    {
        return _color;
    }
}