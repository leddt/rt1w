namespace RTConsole;

public class Camera
{
    private Vec3 _origin;
    private Vec3 _lowerLeftCorner;
    private Vec3 _horizontal;
    private Vec3 _vertical;
    
    public Camera(
        Vec3 lookFrom,
        Vec3 lookAt,
        Vec3 vUp,
        double vfov, 
        double aspectRatio)
    {
        var theta = Utilities.DegreesToRadians(vfov);
        var h = Math.Tan(theta / 2);
        var viewportHeight = 2 * h;
        var viewportWidth = aspectRatio * viewportHeight;

        var w = Vec3.UnitVector(lookFrom - lookAt);
        var u = Vec3.UnitVector(Vec3.Cross(vUp, w));
        var v = Vec3.Cross(w, u);
        
        _origin = lookFrom;
        _horizontal = viewportWidth * u;
        _vertical = viewportHeight * v;
        _lowerLeftCorner = _origin - _horizontal / 2 - _vertical / 2 - w;
    }

    public Ray GetRay(double s, double t)
    {
        return new Ray(
            _origin,
            _lowerLeftCorner + s * _horizontal + t * _vertical - _origin
        );
    }
}