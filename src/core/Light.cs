using System.Numerics;

namespace RaytracingEngine;

public class Light
{
    public Vector3 Position;
    public Vector3 Intensity;

    public Light()
    {
        Position = Vector3.Zero;
        Intensity = new Vector3(1000, 1000, 1000);
    }

    public Light(Vector3 position, Vector3 intensity)
    {
        Position = position;
        this.Intensity = intensity;
    }

    public Vector3 Illumination(Vector3 point, Vector3 normal)
    {
        Ray ray = new Ray(point, Position);
        float distance = ray.distance;
        float cosTheta = Vector3.Dot(ray.direction, normal);

        if (distance == 0)
            return Vector3.Zero;
        if (cosTheta < 0) cosTheta = -cosTheta;
        var result = Intensity * cosTheta / (distance * distance);
        return result;
    }

}