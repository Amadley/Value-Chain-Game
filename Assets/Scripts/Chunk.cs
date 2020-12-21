using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 11/24/2020 neighbor logic on hold.

public class Chunk : MonoBehaviour
{
    // Expose to Inspector for debugging - remove later.
    [SerializeField]
    private Vector2Int normalizedChunkCoordinates;
    public Vector2Int NormalizedChunkCoordinates
    {
        private set
        {
            normalizedChunkCoordinates = value;
        }
        get
        {
            return NormalizedChunkCoordinates;
        }
    }

    //public Dictionary<Direction, Chunk> NeighborChunks { private set; get; }
    public Tile[,] ChunkTiles { private set; get; }
    public ChunkVertex[,] ChunkVertices { set; get; }
    public int ChunkSize { private set; get; }
    public bool IsWorldEdge { set; get; }
    public EdgeStatus edgeStatus { set; get; }
    public float PerlinNoiseScale { set; get; }
    public float MaximumWorldHeight { set; get; }
    public int TileSize { set; get; }

    // Cached References.
    public World World { private set; get; }
    public ChunkMesh ChunkMesh { set; get; }
    public ChunkTileGrid ChunkTileGrid { set; get; }

    public void Awake()
    {
        World = FindObjectOfType<World>();
        CacheWorldData();
        ChunkMesh = GetComponent<ChunkMesh>();
        ChunkTileGrid = GetComponentInChildren<ChunkTileGrid>();

        SetChunkNormalizedCoordinates();

        GenerateChunkTiles();
        GenerateChunkVertices();
        TerrainGeneration.GenerateChunkTerrainData(this);
        TerrainGeneration.DrawChunkTerrain(this);
        ChunkTileGrid = GetComponentInChildren<ChunkTileGrid>();
        ChunkTileGrid.Initialize();
        ChunkTileGrid.DrawChunkGrid(this);
    }

    public void GenerateChunkTiles()
    {
        ChunkTiles = new Tile[ChunkSize, ChunkSize];

        for (int x = 0; x < ChunkSize; x++)
        {
            for (int z = 0; z < ChunkSize; z++)
            {
                Vector2Int tilePositionInChunk = new Vector2Int(x, z);
                Tile tile = new Tile(this, tilePositionInChunk);

                ChunkTiles[x, z] = tile;
            }
        }
    }

    private void GenerateChunkVertices()
    {
        ChunkVertices = new ChunkVertex[ChunkSize + 1, ChunkSize + 1];

        for (int x = 0; x < ChunkSize + 1; x++)
        {
            for (int z = 0; z < ChunkSize + 1; z++)
            {
                Vector2Int VertexFlatWorldCoordinates = new Vector2Int((int)transform.position.x + (x * TileSize), (int)transform.position.z + (z * TileSize));
                ChunkVertex chunkVertex = new ChunkVertex(VertexFlatWorldCoordinates);

                ChunkVertices[x, z] = chunkVertex;
            }
        }
    }

    public void SetChunkNormalizedCoordinates()
    {
        NormalizedChunkCoordinates = new Vector2Int(((int)transform.position.x / (ChunkSize * TileSize)), ((int)transform.position.z / (ChunkSize * TileSize)));
    }

    public void SetTileCornerVertices()
    {
        for (int x = 0; x < ChunkSize; x++)
        {
            for (int z = 0; z < ChunkSize; z++)
            {
                ChunkTiles[x, z].GetCornerVertices();
            }
        }
    }

    public void CacheWorldData()
    {
        ChunkSize = World.ChunkSize;
        PerlinNoiseScale = World.PerlinNoiseScale;
        MaximumWorldHeight = World.MaximumWorldHeight;
        TileSize = World.TileSize;
    }
}