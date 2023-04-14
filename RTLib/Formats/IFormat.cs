namespace RTLib.Formats;

public interface IFormat
{
    void WriteFile(Stream target, Vec3[,] rgbPixels);
}