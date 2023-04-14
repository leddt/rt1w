namespace RTLib.Model;

public struct Ray
{
    public Ray(Vec3 origin, Vec3 direction, double time = 0)
    {
        Origin = origin;
        Direction = direction;
        Time = time;
    }

    public readonly Vec3 Origin;
    public readonly Vec3 Direction;
    public readonly double Time;

    public Vec3 At(double t) => Origin + t * Direction;
}