using System.Numerics;

namespace RaytracingEngine;

public class Camera
{
    private Vector3 _origin; // глаз
    private Vector3 _topLeft; // левый верхний угол
    private int _resolutionX, _resolutionY; // разрешение 
    private Vector3 _dx, _dy; // размера пикселя
    private float _fov;

    public int ResolutionX => _resolutionX;
    public int ResolutionY => _resolutionY;

    public Camera(Vector3 direction, int resX = 1920 , int resY = 1080 , float fovDegrees = 35)
    {
        _origin = new Vector3(-50, 0, 0);
        _resolutionX = resX;
        _resolutionY = resY;
    
        _fov = TransformExtension.ToRadians(fovDegrees);
        float aspectRatio = (float)_resolutionX / _resolutionY;

        Vector3 up = new Vector3(0, 1, 0);
        Vector3 right = Vector3.Cross(up, direction);
    
        // Меняем только проекцию пикселей на плоскость изображения в зависимости от FOV
        float i = 2 * (float)Math.Tan(_fov / 2) * aspectRatio / _resolutionX;
        float j = 2 * (float)Math.Tan(_fov / 2) / _resolutionY;
    
        _topLeft = _origin + direction - right * _resolutionX / 2 * i + up * _resolutionY / 2 * j;
        _dx = right * i;
        _dy = -up * j;
    }



    public Ray CreateRay(int x, int y) {
        var endPoint = _topLeft + x * _dx + y * _dy; 
        return new Ray(_origin, endPoint);
    }
}