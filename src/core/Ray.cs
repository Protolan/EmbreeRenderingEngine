using System.Numerics;

namespace RaytracingEngine;

public class Ray
{
    public Vector3 origin;
    public Vector3 direction;
    public float distance;
    public Vector3 rgb;
    public float indexOfRefraction;
    public Ray(Vector3 origin, Vector3 end)
    {
        this.origin = origin;
        rgb = new Vector3(1, 1, 1);
        var difference = end - origin;
        distance = difference.Length();
        direction = Vector3.Normalize(difference);
        indexOfRefraction = 1f;
    }
}

