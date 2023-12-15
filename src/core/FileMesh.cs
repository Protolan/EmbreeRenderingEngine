using System.Numerics;
using Assimp;

namespace RaytracingEngine;

public class FileMesh: IMesh
{
    public Vector3[] Vertices { get; private set; }
    public int[] Faces { get; private set; }
    
    public FileMesh(string filePath)
    {
        LoadFromFile(filePath, Vector3.One, Vector3.Zero, Vector3.One);
    }

    public FileMesh(string filePath, Vector3 position, Vector3 rotationEuler, Vector3 scale)
    {
        LoadFromFile(filePath, position, rotationEuler, scale);
    }
    
    public FileMesh(string filePath, Vector3 position)
    {
        LoadFromFile(filePath, position, Vector3.Zero, Vector3.One);
    }
    
    public FileMesh(string filePath, Vector3 position, Vector3 rotationEuler)
    {
        LoadFromFile(filePath, position, rotationEuler, Vector3.One);
    }


    private void LoadFromFile(string filePath, Vector3 position, Vector3 rotationEuler, Vector3 scale)
    {
        var importer = new AssimpContext();
        var scene = importer.ImportFile(filePath, PostProcessSteps.Triangulate);

        if (scene == null || scene.SceneFlags.HasFlag(SceneFlags.Incomplete) || scene.RootNode == null)
        {
            throw new InvalidOperationException("Error importing file: " + filePath);
        }

        foreach (var mesh in scene.Meshes)
        {
            Vertices = mesh.Vertices.Select(v =>
                    TransformExtension.TransformVertex(new Vector3(v.X, v.Y, v.Z), position, rotationEuler, scale))
                .ToArray();

            Faces = mesh.Faces
                .Where(f => f.IndexCount == 3)
                .SelectMany(f => f.Indices)
                .ToArray();
        }
    }
}

public interface IMesh
{
    public Vector3[] Vertices { get;  }
    public int[] Faces { get;  }

}