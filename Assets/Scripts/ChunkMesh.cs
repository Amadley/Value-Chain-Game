using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Mesh), typeof(MeshFilter))]
public class ChunkMesh : MonoBehaviour
{
    Mesh mesh;
    List<Vector3> vertices;
    List<int> triangles;
    MeshCollider meshCollider;

    private void Awake()
    {
        Initialize();
    }

    public void Triangulate(Chunk chunk)
    {
        mesh.Clear();
        vertices.Clear();
        triangles.Clear();
        Destroy(meshCollider);


        for (int x = 0; x < chunk.ChunkSize; x++)
        {
            for (int z = 0; z < chunk.ChunkSize; z++)
            {
                Triangulate(chunk.ChunkTiles[x, z]);
            }
        }

        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.RecalculateNormals();

        meshCollider.sharedMesh = mesh;
    }

    public void Initialize()
    {
        mesh = new Mesh();
        mesh = GetComponent<MeshFilter>().mesh;
        meshCollider = gameObject.AddComponent<MeshCollider>();
        mesh.name = "Chunk mesh";
        vertices = new List<Vector3>();
        triangles = new List<int>();
    } 

    private void Triangulate(Tile tile)
    {
        switch (tile.TileChirality)
        {
            case TileChirality.LeftHanded:
                AddTriangle(tile.CornerVertices[TileCorner.BottomLeft].WorldCoordinates, tile.CornerVertices[TileCorner.TopLeft].WorldCoordinates, tile.CornerVertices[TileCorner.TopRight].WorldCoordinates);
                AddTriangle(tile.CornerVertices[TileCorner.TopRight].WorldCoordinates, tile.CornerVertices[TileCorner.BottomRight].WorldCoordinates, tile.CornerVertices[TileCorner.BottomLeft].WorldCoordinates);
                break;
            case TileChirality.RightHanded:
                AddTriangle(tile.CornerVertices[TileCorner.BottomLeft].WorldCoordinates, tile.CornerVertices[TileCorner.TopLeft].WorldCoordinates, tile.CornerVertices[TileCorner.BottomRight].WorldCoordinates);
                AddTriangle(tile.CornerVertices[TileCorner.TopLeft].WorldCoordinates, tile.CornerVertices[TileCorner.TopRight].WorldCoordinates, tile.CornerVertices[TileCorner.BottomRight].WorldCoordinates);
                break;
            default:
                Debug.LogError("Unhandled case in ChunkMesh.Triangulate().");
                break;
        }
    }

    private void AddTriangle(Vector3 v1, Vector3 v2, Vector3 v3)
    {
        int vertexIndex = vertices.Count;

        vertices.Add(transform.InverseTransformPoint(v1));
        vertices.Add(transform.InverseTransformPoint(v2));
        vertices.Add(transform.InverseTransformPoint(v3));
        triangles.Add(vertexIndex);
        triangles.Add(vertexIndex + 1);
        triangles.Add(vertexIndex + 2);
    }
}