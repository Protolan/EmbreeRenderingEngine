using System.Numerics;

namespace RaytracingEngine;

public static class TransformExtension
{
    public static Vector3 TransformVertex(Vector3 vertex, Vector3 position, Vector3 rotationEuler, Vector3 scale)
    {
        // Преобразование: масштабирование -> вращение -> перемещение
        Matrix4x4 transformation = Matrix4x4.Identity
                                   * Matrix4x4.CreateScale(scale.X, scale.Y, scale.Z)
                                   * Matrix4x4.CreateRotationX(ToRadians(rotationEuler.X))
                                   * Matrix4x4.CreateRotationY(ToRadians(rotationEuler.Y))
                                   * Matrix4x4.CreateRotationZ(ToRadians(rotationEuler.Z))
                                   * Matrix4x4.CreateTranslation(position.X, position.Y, position.Z);

        return Vector3.Transform(new Vector3(vertex.X, vertex.Y, vertex.Z), transformation);
    }
    
    public static Vector3 RotateAxis(this Vector3 vector3, Vector3 axis, float angles)
    {
        var rotation = Quaternion.CreateFromAxisAngle(axis, ToRadians(angles));
        return Vector3.Transform(vector3, rotation);
    }

    public static Vector3 RotateX(this Vector3 vector3, float angles) => vector3.RotateAxis(Vector3.UnitX, angles);

    public static Vector3 RotateY(this Vector3 vector3, float angles) => vector3.RotateAxis(Vector3.UnitY, angles);

    public static Vector3 RotateZ(this Vector3 vector3, float angles) => vector3.RotateAxis(Vector3.UnitZ, angles);
    public static float ToRadians(float angleX) => (float)(angleX * Math.PI / 180);

    private static Quaternion LookAt(this Vector3 sourcePoint, Vector3 destPoint)
    {
        Vector3 forwardVector = Vector3.Normalize(destPoint - sourcePoint);

        float dot = Vector3.Dot(Vector3.UnitZ, forwardVector);

        if (Math.Abs(dot - (-1.0f)) < 0.000001f)
        {
            // Vector points directly at -Z
            return new Quaternion(Vector3.UnitY, MathF.PI);
        }
        if (Math.Abs(dot - (1.0f)) < 0.000001f)
        {
            // Vector points directly at Z
            return Quaternion.Identity;
        }

        float rotAngle = (float)Math.Acos(dot);
        Vector3 rotAxis = Vector3.Cross(Vector3.UnitZ, forwardVector);
        rotAxis = Vector3.Normalize(rotAxis);
        return Quaternion.CreateFromAxisAngle(rotAxis, rotAngle);
    }
}