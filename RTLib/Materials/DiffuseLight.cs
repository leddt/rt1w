using RTLib.Model;
using RTLib.Textures;

namespace RTLib.Materials;

public class DiffuseLight : IMaterial
{
    private readonly ITexture _texture;

    public DiffuseLight(Vec3 color) : this(new SolidColor(color))
    {
    }

    public DiffuseLight(ITexture texture)
    {
        _texture = texture;
    }

    public bool Scatter(Ray rIn, Hit rec, out Vec3 attenuation, out Ray scattered)
    {
        attenuation = new Vec3();
        scattered = new Ray();
        return false;
    }

    public Vec3 Emitted(double u, double v, Vec3 p)
    {
        return _texture.Value(u, v, p);
    }
}