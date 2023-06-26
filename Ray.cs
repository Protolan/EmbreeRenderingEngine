using System.Numerics;

namespace RaytracingEngine;

public class Ray
{
    public Vector3 start;
    public Vector3 direction;
    public float distance;
    public Ray(Vector3 start, Vector3 end)
    {
        this.start = start;
        var difference = end - start;
        distance = difference.Length();
        direction = Vector3.Normalize(difference);
    }
}