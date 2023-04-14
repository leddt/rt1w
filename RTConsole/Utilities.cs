﻿namespace RTConsole;

public static class Utilities
{
    public static double DegreesToRadians(double degrees) => degrees * Math.PI / 180;
    public static double NextDouble(this Random rng, double min, double max) => min + (max - min) * rng.NextDouble();
}