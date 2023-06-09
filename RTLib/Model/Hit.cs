﻿using RTLib.Materials;

namespace RTLib.Model;

public struct Hit
{
    public Vec3 P;
    public Vec3 Normal;
    public IMaterial Material;
    public double T;
    public double U;
    public double V;
    public bool FrontFace;

    public void SetFaceNormal(Ray r, Vec3 outwardNormal)
    {
        FrontFace = Vec3.Dot(r.Direction, outwardNormal) < 0;
        Normal = FrontFace ? outwardNormal : -outwardNormal;
    }
}