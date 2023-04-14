namespace RTConsole;

public struct Ray
{
    public Ray(Vec3 origin, Vec3 direction)
    {
        Origin = origin;
        Direction = direction;
    }

    public Vec3 Origin { get; }
    public Vec3 Direction { get; }

    public Vec3 At(double t) => Origin + t * Direction;
}