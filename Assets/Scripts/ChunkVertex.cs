using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkVertex
{
    public Vector2Int FlatWorldCoordinates { set; get; }
    public int NormalizedElevation { set; get; }
    public Vector3 WorldCoordinates { set; get; }

    public ChunkVertex(Vector2Int flatWorldCoordinates)
    {
        FlatWorldCoordinates = flatWorldCoordinates;
    }
}