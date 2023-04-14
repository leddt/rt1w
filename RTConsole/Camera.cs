namespace RTConsole;

public class Camera
{
    private Vec3 _origin;
    private Vec3 _lowerLeftCorner;
    private Vec3 _horizontal;
    private Vec3 _vertical;
    
    public Camera()
    {
        var aspectRatio = 16.0 / 9.0;
        var viewportHeight = 2.0;
        var viewportWidth = aspectRatio * viewportHeight;
        var focalLength = 1.0;
        
        _origin = new Vec3(0, 0, 0);
        _horizontal = new Vec3(viewportWidth, 0, 0);
        _vertical = new Vec3(0, viewportHeight, 0);
        _lowerLeftCorner = _origin - _horizontal / 2 - _vertical / 2 - new Vec3(0, 0, focalLength);
    }

    public Ray GetRay(double u, double v)
    {
        return new Ray(
            _origin,
            _lowerLeftCorner + u * _horizontal + v * _vertical - _origin
        );
    }
}