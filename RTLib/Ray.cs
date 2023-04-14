namespace RTLib;

public struct Ray
{
    public Ray(Vec3 origin, Vec3 direction)
    {
        Origin = origin;
        Direction = direction;
    }

    public readonly Vec3 Origin;
    public readonly Vec3 Direction;

    public Vec3 At(double t) => Origin + t * Direction;
}