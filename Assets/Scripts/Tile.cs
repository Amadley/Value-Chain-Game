using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum TileCorner
{
    TopLeft,
    TopRight,
    BottomLeft,
    BottomRight
}

public enum TileChirality
{
    LeftHanded,
    RightHanded
}

public class Tile
{
    public TileChirality TileChirality { set; get; }
    public Chunk ParentChunk { set; get; }
    public bool IsChunkEdge { set; get; }
    public EdgeStatus edgeStatus { set; get; }
    public float TileSize { set; get; }
    public Vector2Int PositionInChunk { set; get; }
    public Vector3 PositionInWorld { set; get; }
    public float MeanElevationLevel { set; get; }
    //public Dictionary<Direction, Tile> NeighborTiles { set; get; }
    public Dictionary<TileCorner, ChunkVertex> CornerVertices { private set; get; }

    // Methods.
    private Tile() { }
    public Tile(Chunk parentChunk, Vector2Int positionInChunk)
    {
        ParentChunk = parentChunk;

        PositionInChunk = positionInChunk;
        DeterminePositionInWorld(); 

        TileSize = ParentChunk.TileSize;
    }

    public void GetCornerVertices()
    {
        CornerVertices = new Dictionary<TileCorner, ChunkVertex>();

        CornerVertices.Add(TileCorner.TopLeft, ParentChunk.ChunkVertices[PositionInChunk.x, PositionInChunk.y + 1]);
        CornerVertices.Add(TileCorner.TopRight, ParentChunk.ChunkVertices[PositionInChunk.x + 1, PositionInChunk.y + 1]);
        CornerVertices.Add(TileCorner.BottomLeft, ParentChunk.ChunkVertices[PositionInChunk.x, PositionInChunk.y]);
        CornerVertices.Add(TileCorner.BottomRight, ParentChunk.ChunkVertices[PositionInChunk.x + 1, PositionInChunk.y]);
    }

    public void DeterminePositionInWorld()
    {
        PositionInWorld = new Vector3(ParentChunk.transform.position.x + PositionInChunk.x, MeanElevationLevel, ParentChunk.transform.position.z + PositionInChunk.y);  
    }

    public void SetMeanElevationLevel()
    {
        MeanElevationLevel = (CornerVertices[TileCorner.TopLeft].WorldCoordinates.y + CornerVertices[TileCorner.TopRight].WorldCoordinates.y + CornerVertices[TileCorner.BottomLeft].WorldCoordinates.y + CornerVertices[TileCorner.BottomRight].WorldCoordinates.y) / 4;
    }
}