namespace RTLib.Textures;

public interface ITexture
{
    Vec3 Value(double u, double v, Vec3 p);
}