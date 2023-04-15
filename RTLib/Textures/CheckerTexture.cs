namespace RTLib.Textures;

public class CheckerTexture : ITexture
{
    private readonly ITexture _even;
    private readonly ITexture _odd;
    private readonly double _scale;

    public CheckerTexture(Vec3 even, Vec3 odd, double scale = 10) 
        : this(new SolidColor(even), new SolidColor(odd), scale)
    {
    }

    public CheckerTexture(ITexture even, ITexture odd, double scale = 10)
    {
        _even = even;
        _odd = odd;
        _scale = scale;
    }

    public Vec3 Value(double u, double v, Vec3 p)
    {
        var sines = Math.Sin(_scale * p.X) *
                    Math.Sin(_scale * p.Y) *
                    Math.Sin(_scale * p.Z);
        
        return sines < 0 
            ? _odd.Value(u, v, p) 
            : _even.Value(u, v, p);
    }
}