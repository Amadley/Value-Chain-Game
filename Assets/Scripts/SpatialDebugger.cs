using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpatialDebugger : MonoBehaviour
{
    public GameObject redDebugSphere;
    public GameObject orangeDebugSphere;
    public GameObject yellowDebugSphere;
    public GameObject greenDebugSphere;
    public GameObject blueDebugSphere;

    // Cache a reference to the world;
    World world;

    void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        world = FindObjectOfType<World>();
        //InstantiateRedSpheresAtChunkTransforms();
        //InstantiateOrangeSpheresAtTileCoordinates();
        //InstantiateYellowSpheresAtVertexFlatCoordinates();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void InstantiateRedSpheresAtChunkTransforms()
    {
        foreach (Chunk chunk in world.ActiveChunks)
        {
            Instantiate(redDebugSphere, chunk.transform.position, Quaternion.identity);
        }
    }

    private void InstantiateOrangeSpheresAtTileCoordinates()
    {
        foreach (Chunk chunk in world.ActiveChunks)
        {
            foreach (Tile tile in chunk.ChunkTiles)
            {
                Instantiate(orangeDebugSphere, tile.PositionInWorld, Quaternion.identity);
            }
        }
    }

    private void InstantiateYellowSpheresAtVertexFlatCoordinates()
    {
        foreach (Chunk chunk in world.ActiveChunks)
        {
            foreach (Tile tile in chunk.ChunkTiles)
            {
                ChunkVertex cv1 = tile.CornerVertices[TileCorner.TopLeft];
                ChunkVertex cv2 = tile.CornerVertices[TileCorner.TopRight];
                ChunkVertex cv3 = tile.CornerVertices[TileCorner.BottomLeft];
                ChunkVertex cv4 = tile.CornerVertices[TileCorner.BottomRight];

                Instantiate(yellowDebugSphere, cv1.WorldCoordinates, Quaternion.identity);
                Instantiate(yellowDebugSphere, cv2.WorldCoordinates, Quaternion.identity);
                Instantiate(yellowDebugSphere, cv3.WorldCoordinates, Quaternion.identity);
                Instantiate(yellowDebugSphere, cv4.WorldCoordinates, Quaternion.identity);
            }
        }
    }
}
