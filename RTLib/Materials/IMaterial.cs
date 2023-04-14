namespace RTLib.Materials;

public interface IMaterial
{
    bool Scatter(Ray rIn, Hit rec, out Vec3 attenuation, out Ray scattered);
}