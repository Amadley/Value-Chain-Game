using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TerrainGeneration 
{
    public static void GenerateChunkTerrainData(Chunk chunk)
    {
        SetChunkVerticesElevation(chunk);
        chunk.SetTileCornerVertices();
        SetTileChirality(chunk);
        SetTileMeanElevation(chunk);
    }

    public static void DrawChunkTerrain(Chunk chunk)
    {
        chunk.ChunkMesh.Initialize();
        chunk.ChunkMesh.Triangulate(chunk);
    }

    private static void SetChunkVerticesElevation(Chunk chunk)
    {
        if (chunk.ChunkVertices == null)
        {
            Debug.LogError("ChunkVertices have not been instantiated at time of TerrainGeneration.AssignNoiseValuesToVertices call from chunk: " + chunk.transform.position.x + ", " + chunk.transform.position.z);
        }
        else
        {
            float[,] NoiseMap = new float[chunk.ChunkSize + 1, chunk.ChunkSize + 1];

            for (int x = 0; x < chunk.ChunkSize + 1; x++)
            {
                for (int z = 0; z < chunk.ChunkSize + 1; z++)
                {
                    // Step one: determine noisemap values.
                    float xSample = (((x + chunk.World.PerlinXOffset) * chunk.World.TileSize) + chunk.transform.position.x) * chunk.PerlinNoiseScale;
                    float zSample = (((z + chunk.World.perlinZOffset) * chunk.World.TileSize) + chunk.transform.position.z) * chunk.PerlinNoiseScale;

                    NoiseMap[x, z] = Mathf.PerlinNoise(xSample, zSample); /*+ ((1 / 2) * Mathf.PerlinNoise(2 * xSample, 2 * zSample)) + ((1 / 4) * Mathf.PerlinNoise(4 * xSample, 4 * zSample));*/

                    // Step two: Set noisemap value as elevation for each vertex.
                    float rawElevation = NoiseMap[x, z];
                    
                    // Step three: Depending on whether discrete or non discrete elevation is selected in inspector, set elevation of each Chunk Vertex.
                    if (chunk.World.DiscreteElevationIncrements)
                    {
                        float threshold = 1 / chunk.World.MaximumWorldHeight;
                        int unitsElevation = (int)(rawElevation / threshold);
                        float finalElevation = unitsElevation * chunk.World.UnitElevationMagnitude;
                        chunk.ChunkVertices[x, z].WorldCoordinates = new Vector3(chunk.transform.position.x + (x * chunk.TileSize), finalElevation, chunk.transform.position.z + (z * chunk.TileSize));
                    }
                    else
                    {
                        rawElevation *= chunk.World.MaximumWorldHeight;
                        chunk.ChunkVertices[x, z].WorldCoordinates = new Vector3(chunk.transform.position.x + (x * chunk.TileSize), rawElevation, chunk.transform.position.z + (z * chunk.TileSize));
                    }
                    
                    // Step four: Set the mean tile elevation that will be used to place units on tile.

                }
            }
        } 
    }

    // Certain tiles need to have their mesh triangles drawn differently in order to produce consistent looking terrain. These tiles are mirror images of one another, and the manner in which the triangles are drawn are also mirror images. This code determines which version should be used.
    // A left handed tile refers to a square tile that divides itself into two triangles with a line from the bottom left corner to the top right corner for purposes of drawing the mesh. This is the minority case.
    // A right handed tile refers to a square tile that divides itself into two triangles with a line from the top right corner to the bottom left corner for purposes of drawing the mesh. This is the majority case.
    private static void SetTileChirality(Chunk chunk)
    {
        if (chunk.World.DiscreteElevationIncrements && chunk.World.ConsiderTileChirality)
        {
            foreach (Tile tile in chunk.ChunkTiles)
            {
                float topLeftElevation = tile.CornerVertices[TileCorner.TopLeft].WorldCoordinates.y;
                float topRightElevation = tile.CornerVertices[TileCorner.TopRight].WorldCoordinates.y;
                float bottomLeftElevation = tile.CornerVertices[TileCorner.BottomLeft].WorldCoordinates.y;
                float bottomRightElevation = tile.CornerVertices[TileCorner.BottomRight].WorldCoordinates.y;

                if ((topLeftElevation == bottomLeftElevation) && (bottomLeftElevation == bottomRightElevation) && (topRightElevation != bottomRightElevation))
                {
                    tile.TileChirality = TileChirality.RightHanded;
                }
                else if ((topLeftElevation == topRightElevation) && (topRightElevation == bottomRightElevation) && (bottomRightElevation != bottomLeftElevation))
                {
                    tile.TileChirality = TileChirality.RightHanded;
                }
                else
                {
                    tile.TileChirality = TileChirality.LeftHanded;
                }
            }
        }
    }

    private static void SetTileMeanElevation(Chunk chunk)
    {
        foreach (Tile tile in chunk.ChunkTiles)
        {
            tile.SetMeanElevationLevel();
        }
    }
}