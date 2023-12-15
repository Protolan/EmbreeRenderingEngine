using TinyEmbree;

namespace RaytracingEngine;

public class Geometry
{
    public Geometry()
    {
        Raytracer rt = new();
        
        var sphere = new FileMesh("sphere.fbx");
        rt.AddMesh(new TriangleMesh(sphere.Vertices, sphere.Faces));
        
        rt.CommitScene();
    }
}