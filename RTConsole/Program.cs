using RTConsole;

// Image
const double aspectRatio = 16.0 / 9.0;
const int imageWidth = 400;
const int imageHeight = (int)(imageWidth / aspectRatio);

// World
var world = new HittableList
{
    new Sphere(new Vec3(0, 0, -1), 0.5),
    new Sphere(new Vec3(0, -100.5, -1), 100)
};

// Camera
var viewportHeight = 2.0;
var viewportWidth = aspectRatio * viewportHeight;
var focalLength = 1.0;

var origin = new Vec3(0, 0, 0);
var horizontal = new Vec3(viewportWidth, 0, 0);
var vertical = new Vec3(0, viewportHeight, 0);
var lowerLeftCorner = origin - horizontal / 2 - vertical / 2 - new Vec3(0, 0, focalLength);

// Render

Console.Write($"P3\n{imageWidth} {imageHeight}\n255\n");

for (var j = imageHeight - 1; j >= 0; j--)
{
    Console.Error.Write($"\rScanlines remaining: {j} ");
    for (var i = 0; i < imageWidth; i++)
    {
        var u = (double)i / (imageWidth - 1);
        var v = (double)j / (imageHeight - 1);
        var r = new Ray(origin, lowerLeftCorner + u * horizontal + v * vertical - origin);
        
        var pixelColor = RayColor(r, world);
        pixelColor.WriteColor(Console.Out);
    }
}

Console.Error.Write("\nDone.\n");

Vec3 RayColor(Ray r, IHittable world)
{
    var rec = new Hit();
    if (world.Hit(r, 0, double.PositiveInfinity, ref rec))
    {
        return 0.5 * (rec.Normal + new Vec3(1, 1, 1));
    }
    
    var unitDirection = Vec3.UnitVector(r.Direction);
    var t = 0.5 * (unitDirection.Y + 1);
    return (1 - t) * new Vec3(1, 1, 1) + t * new Vec3(0.5, 0.7, 1);
}

double DegreesToRadians(double degrees) => degrees * Math.PI / 180;