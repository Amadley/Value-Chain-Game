using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Mesh), typeof(MeshFilter))]
public class ChunkTileGrid : MonoBehaviour
{
    public float LineWidth = 0.1f;
    public float OffsetAboveTerrainMesh = 0.01f;

    Mesh mesh;
    List<Vector3> vertices;
    List<int> triangles;

    public void Initialize()
    {
        mesh = new Mesh();
        mesh = GetComponent<MeshFilter>().mesh;
        mesh.name = "Chunk tile grid mesh";
        vertices = new List<Vector3>();
        triangles = new List<int>();
    }

    public void DrawChunkGrid(Chunk chunk)
    {
        mesh.Clear();
        vertices.Clear();
        triangles.Clear();

        // Draw thin lines connecting each tiles' bottom left corner vertex to the bottom right corner vertex, and the bottom right corner vertex to the top right corner vertex.
        // Lines are rendered as thin quads.
        for (int x = 0; x < chunk.ChunkSize; x++)
        {
            for (int z = 0; z < chunk.ChunkSize; z++)
            {
                Tile tile = chunk.ChunkTiles[x, z];

                DrawLineAlongBottomOfTile(tile);
                DrawLineAlongRightSideOfTile(tile);

                if (x == 0)
                {
                    DrawLineAlongLeftSideOfTile(tile);
                }

                if (z == chunk.ChunkSize - 1)
                {
                    DrawLineALongTopOfTile(tile);
                }
            }
        }

        

        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.RecalculateNormals();
    }

    private void DrawLineAlongBottomOfTile(Tile tile)
    {
        Vector3[] tileVertexCoordinates = { tile.CornerVertices[TileCorner.BottomLeft].WorldCoordinates,
                                            tile.CornerVertices[TileCorner.BottomRight].WorldCoordinates };

        Vector3 v1 = tileVertexCoordinates[0] + new Vector3(0, OffsetAboveTerrainMesh, LineWidth / 2);
        Vector3 v2 = tileVertexCoordinates[0] + new Vector3(0, OffsetAboveTerrainMesh, -LineWidth / 2);
        Vector3 v3 = tileVertexCoordinates[1] + new Vector3(0, OffsetAboveTerrainMesh, LineWidth / 2);
        Vector3 v4 = tileVertexCoordinates[1] + new Vector3(0, OffsetAboveTerrainMesh, -LineWidth / 2);

        AddTriangle(v1, v3, v4);
        AddTriangle(v1, v4, v2);
    }

    private void DrawLineAlongRightSideOfTile(Tile tile)
    {
        Vector3[] tileVertexCoordinates = { tile.CornerVertices[TileCorner.BottomRight].WorldCoordinates,
                                            tile.CornerVertices[TileCorner.TopRight].WorldCoordinates };

        Vector3 v1 = tileVertexCoordinates[0] + new Vector3(-LineWidth / 2, OffsetAboveTerrainMesh, 0);
        Vector3 v2 = tileVertexCoordinates[0] + new Vector3(LineWidth / 2, OffsetAboveTerrainMesh, 0);
        Vector3 v3 = tileVertexCoordinates[1] + new Vector3(-LineWidth / 2, OffsetAboveTerrainMesh, 0);
        Vector3 v4 = tileVertexCoordinates[1] + new Vector3(LineWidth / 2, OffsetAboveTerrainMesh, 0);

        AddTriangle(v1, v3, v4);
        AddTriangle(v1, v4, v2);
    }

    private void DrawLineAlongLeftSideOfTile(Tile tile)
    {
        Vector3[] tileVertexCoordinates = { tile.CornerVertices[TileCorner.BottomLeft].WorldCoordinates,
                                            tile.CornerVertices[TileCorner.TopLeft].WorldCoordinates };

        Vector3 v1 = tileVertexCoordinates[0] + new Vector3(-LineWidth / 2, OffsetAboveTerrainMesh, 0);
        Vector3 v2 = tileVertexCoordinates[0] + new Vector3(LineWidth / 2, OffsetAboveTerrainMesh, 0);
        Vector3 v3 = tileVertexCoordinates[1] + new Vector3(-LineWidth / 2, OffsetAboveTerrainMesh, 0);
        Vector3 v4 = tileVertexCoordinates[1] + new Vector3(LineWidth / 2, OffsetAboveTerrainMesh, 0);

        AddTriangle(v1, v3, v4);
        AddTriangle(v1, v4, v2);
    }

    private void DrawLineALongTopOfTile(Tile tile)
    {
        Vector3[] tileVertexCoordinates = { tile.CornerVertices[TileCorner.TopLeft].WorldCoordinates,
                                            tile.CornerVertices[TileCorner.TopRight].WorldCoordinates };

        Vector3 v1 = tileVertexCoordinates[0] + new Vector3(0, OffsetAboveTerrainMesh, LineWidth / 2);
        Vector3 v2 = tileVertexCoordinates[0] + new Vector3(0, OffsetAboveTerrainMesh, -LineWidth / 2);
        Vector3 v3 = tileVertexCoordinates[1] + new Vector3(0, OffsetAboveTerrainMesh, LineWidth / 2);
        Vector3 v4 = tileVertexCoordinates[1] + new Vector3(0, OffsetAboveTerrainMesh, -LineWidth / 2);

        AddTriangle(v1, v3, v4);
        AddTriangle(v1, v4, v2);
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
