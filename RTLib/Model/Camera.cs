namespace RTLib.Model;

public class Camera
{
    private Vec3 _origin;
    private Vec3 _lowerLeftCorner;
    private Vec3 _horizontal;
    private Vec3 _vertical;

    private Vec3 _w, _u, _v;
    private double _lensRadius;
    
    public Camera(
        Vec3 lookFrom,
        Vec3 lookAt,
        Vec3 vUp,
        double vfov, 
        double aspectRatio,
        double aperture,
        double focusDist)
    {
        var theta = Utils.DegreesToRadians(vfov);
        var h = Math.Tan(theta / 2);
        var viewportHeight = 2 * h;
        var viewportWidth = aspectRatio * viewportHeight;

        _w = Vec3.UnitVector(lookFrom - lookAt);
        _u = Vec3.UnitVector(Vec3.Cross(vUp, _w));
        _v = Vec3.Cross(_w, _u);
        
        _origin = lookFrom;
        _horizontal = focusDist * viewportWidth * _u;
        _vertical = focusDist * viewportHeight * _v;
        _lowerLeftCorner = _origin - _horizontal / 2 - _vertical / 2 - focusDist * _w;

        _lensRadius = aperture / 2;
    }

    public Ray GetRay(double s, double t)
    {
        var rd = _lensRadius * Vec3.RandomInUnitDisk();
        var offset = _u * rd.X + _v * rd.Y;
        
        return new Ray(
            _origin + offset,
            _lowerLeftCorner + s * _horizontal + t * _vertical - _origin - offset
        );
    }
}