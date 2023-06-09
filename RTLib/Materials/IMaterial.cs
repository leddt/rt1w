﻿using RTLib.Model;

namespace RTLib.Materials;

public interface IMaterial
{
    bool Scatter(Ray rIn, Hit rec, out Vec3 attenuation, out Ray scattered);
    Vec3 Emitted(double u, double v, Vec3 p) => new();
}