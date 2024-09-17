using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace RTLib.Textures;

public class ImageTexture : ITexture
{
    private readonly Image<Rgb24> _image;
    private readonly int _width;
    private readonly int _height;

    public ImageTexture(string filename)
    {
        _image = Image.Load<Rgb24>(filename);
        _width = _image.Width;
        _height = _image.Height;
    }

    public Vec3 Value(double u, double v, Vec3 p)
    {
        u = Math.Clamp(u, 0, 1);
        v = 1 - Math.Clamp(v, 0, 1); // Flip V to image coordinates

        var i = (int)(u * _width);
        var j = (int)(v * _height);

        // Clamp integer mapping, since actual coordinates should be less than 1.0
        if (i >= _width) i = _width - 1;
        if (j >= _height) j = _height - 1;

        const double colorScale = 1.0 / 255.0;
        var pixel = _image[i, j];

        return new Vec3(colorScale * pixel.R, colorScale * pixel.G, colorScale * pixel.B);
    }
}