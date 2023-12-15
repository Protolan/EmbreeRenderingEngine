using System.Numerics;
using RaytracingEngine.appearances;
using RaytracingEngine.extensions;

namespace RaytracingEngine;

public class OpticalProperties
{
    public Vector3 Color { get; set; }
    public bool IsLightSource { get; set; }
    private readonly IAppearance[] _appearances;

    public OpticalProperties()
    {
        Color = new Vector3(255, 255, 255) / 255; // Нормализация цвета
        IsLightSource = false;

        // Инициализация различных видов взаимодействия света с поверхностью
        _appearances = new IAppearance[4];
        _appearances[0] = new MirrorReflection();
        _appearances[1] = new DiffuseReflection();
        _appearances[2] = new MirrorRefraction();
        _appearances[3] = new DiffuseRefraction();
    }

    public OpticalProperties(Vector3 color, bool isLightSource, params IAppearance[] appearances)
    {
        Color = color;
        IsLightSource = isLightSource;
        _appearances = appearances;
    }

    public int Diffusion(Ray ray, Vector3 normal)
    {
        double probability = RandomHelper.RandomDouble() * 1.2;
        if (probability > 1)
        {
            return -1; // Поглощение луча
        }

        for (int i = 0; i < _appearances.Length; i++)
        {
            probability = _appearances[i].ChooseEvent(probability, ray);
            if (probability <= 0)
            {
                return _appearances[i].Diffusion(ray, normal);
            }
        }

        return -1; // Поглощение луча, если ни одно событие не выбрано
    }

    public Vector3 Luminance(Vector3 e) => Vector3.Multiply(Color, e) / MathF.PI;
}