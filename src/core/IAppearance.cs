using System.Numerics;
using RaytracingEngine.extensions;

namespace RaytracingEngine.appearances;

public interface IAppearance
{
    public double ChooseEvent(double r, Ray ray);
    public int Diffusion(Ray ray, Vector3 normal);
    public Vector3 Luminance();
}

public class MirrorReflection : IAppearance
{
    private Vector3 _ks; // Коэффициент зеркального отражения

    public MirrorReflection()
    {
        _ks = Vector3.Zero;
    }

    public MirrorReflection(Vector3 ks)
    {
        _ks = ks;
    }

    public double ChooseEvent(double r, Ray ray)
    {
        double x = Vector3.Dot(ray.rgb, _ks);
        r -= x;
        return r;
    }

    public int Diffusion(Ray ray, Vector3 normal)
    {
        ray.direction -= 2 * Vector3.Dot(ray.direction, normal) * normal;
        return 0; // Можно использовать enum для различных типов диффузии
    }

    public Vector3 Luminance()
    {
        return Vector3.Zero;
    }
}

public class DiffuseReflection : IAppearance
{
    public Vector3 DiffuseCoefficient { get; set; }

    public DiffuseReflection()
    {
        DiffuseCoefficient = Vector3.Zero;
    }

    public DiffuseReflection(Vector3 diffuseCoefficient)
    {
        DiffuseCoefficient = diffuseCoefficient;
    }

    public double ChooseEvent(double probability, Ray ray)
    {
        double intensityFactor = Vector3.Dot(ray.rgb, DiffuseCoefficient);
        return probability - intensityFactor;
    }

    public int Diffusion(Ray ray, Vector3 normal)
    {
        ray.direction = RandomHelper.RandomRay(ray.origin, normal).direction;
        return 1;
    }

    public Vector3 Luminance()
    {
        return Vector3.Zero;
    }
}

public class MirrorRefraction : IAppearance
{
    public Vector3 RefractionCoefficient { get; set; }
    public float IndexOfRefraction1 { get; set; }
    public float IndexOfRefraction2 { get; set; }

    public MirrorRefraction()
    {
        RefractionCoefficient = Vector3.Zero;
        IndexOfRefraction1 = 1.0f;
        IndexOfRefraction2 = 1.0f;
    }

    public MirrorRefraction(Vector3 refractionCoefficient, float n1, float n2)
    {
        RefractionCoefficient = refractionCoefficient;
        IndexOfRefraction1 = n1;
        IndexOfRefraction2 = n2;
    }

    public double ChooseEvent(double probability, Ray ray)
    {
        double intensityFactor = Vector3.Dot(ray.rgb, RefractionCoefficient);
        return probability - intensityFactor;
    }

    public int Diffusion(Ray ray, Vector3 normal)
    {
        if (ray.indexOfRefraction == IndexOfRefraction1)
        {
            ray.direction = CalculateRefractionDirection(ray.direction, normal, IndexOfRefraction1, IndexOfRefraction2);
            ray.indexOfRefraction = IndexOfRefraction2;
        }
        else
        {
            ray.direction = CalculateRefractionDirection(ray.direction, normal, IndexOfRefraction2, IndexOfRefraction1);
            ray.indexOfRefraction = IndexOfRefraction1;
        }

        return 2; 
    }

    public Vector3 Luminance()
    {
        return Vector3.Zero;
    }

    private Vector3 CalculateRefractionDirection(Vector3 direction, Vector3 normal, float n1, float n2)
    {
        var refractiveIndexRatio = n1 / n2;
        var cosTheta = -Vector3.Dot(normal, direction);
        var rOutParallel = refractiveIndexRatio * (direction + cosTheta * normal);
        var rOutPerp = -MathF.Sqrt(1.0f - rOutParallel.LengthSquared()) * normal;
        return rOutParallel + rOutPerp;
    }
}

public class DiffuseRefraction : IAppearance
{
    public Vector3 RefractionCoefficient { get; set; }

    public DiffuseRefraction()
    {
        RefractionCoefficient = Vector3.Zero;
    }

    public DiffuseRefraction(Vector3 refractionCoefficient)
    {
        RefractionCoefficient = refractionCoefficient;
    }

    public double ChooseEvent(double probability, Ray ray)
    {
        double intensityFactor = Vector3.Dot(ray.rgb, RefractionCoefficient);
        return probability - intensityFactor;
    }

    public int Diffusion(Ray ray, Vector3 normal)
    {
        ray.direction = -RandomHelper.RandomRay(ray.origin, normal).direction;
        return 3; 
    }

    public Vector3 Luminance() => Vector3.Zero;
}