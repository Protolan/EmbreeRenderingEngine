using System.Numerics;
using Assimp;
using RaytracingEngine;
using TinyEmbree;
using Camera = RaytracingEngine.Camera;
using Light = RaytracingEngine.Light;
using Ray = TinyEmbree.Ray;


Console.WriteLine("Введите путь к файлу:");
string filePath = Console.ReadLine();


var scene = new AssimpContext().ImportFile(filePath, PostProcessSteps.Triangulate);
Raytracer rt = new();
Vector3D sum = new Vector3D(0, 0, 0);
int count = 0;

foreach (var mesh in scene.Meshes)
{
    foreach (var vertex in mesh.Vertices)
    {
        sum += vertex;
        count++;
    }
}

var pivot = sum / count;
Console.WriteLine(pivot.ToString());
foreach (var mesh in scene.Meshes)
{
    Vector3[] vertices = mesh.Vertices.Select(v => new Vector3(v.X, v.Y, v.Z).RotateZ(90).RotateX(90)).ToArray();
    int[] faces = mesh.Faces.SelectMany(f => f.Indices).ToArray();
    rt.AddMesh(new TriangleMesh(vertices, faces));
}

rt.CommitScene(); 


var camera = new Camera(Vector3.UnitX);
// var light1 = new Light(new Vector3(3, 0, -7), new Vector3(12800, 30000, 10000));
var light2 = new Light(new Vector3(-20, 0, 0), new Vector3(255, 255, 255));
var op = new OpticalProperties(new Vector3(255, 255, 255));
Vector3[,] image = new Vector3[camera.ResolutionX, camera.ResolutionY];

for (int i = 0; i < camera.ResolutionX; i++) {
    for (int j = 0; j < camera.ResolutionY; j++) {
        var ray = camera.CreateRay(i, j);
        
        var hit = rt.Trace(new Ray
        {
            Origin = ray.start,
            Direction = ray.direction,
            MinDistance = 0
        });

        if (hit.Mesh != null)
        {
          
            // image[i, j] += op.Brightness(light1.Illumination(hit.Position, hit.Normal));
            image[i, j] += op.Brightness(light2.Illumination(hit.Position, hit.Normal));
        }
    }

}
ImageCreator.CreatePng(image, "render.png");
