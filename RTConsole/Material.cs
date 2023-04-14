namespace RTConsole;

public abstract class Material
{
    public abstract bool Scatter(Ray rIn, Hit rec, Vec3 attenuation, out Ray scattered);
}