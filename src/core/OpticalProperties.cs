using System.Numerics;
using static System.Single;

namespace RaytracingEngine;

public class OpticalProperties
{
    private readonly Vector3 _color;
    
    public OpticalProperties() => _color = new Vector3(255, 255, 255);

    public OpticalProperties(Vector3 color) => _color = color;

    public Vector3 Brightness(Vector3 E) => _color * (E / Pi);
}