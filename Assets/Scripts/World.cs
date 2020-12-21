using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// 11/24/2020 neighbor logic on hold.

public class World : MonoBehaviour
{

    // Chunk prefab and list of active chunks. - Consider a different data structure. Accessing active chunks to activate / deactivate is not super efficient with list - consider refactor.
    public Chunk ChunkPrefab;

    // Use for debugging.
    public GameObject redDebugSphere;
    public GameObject orangeDebugSphere;
    public GameObject yellowDebugSphere;
    public GameObject greenDebugSphere;
    public GameObject blueDebugSphere;

    public List<Chunk> ActiveChunks { set; get; }

    // These variables help features of WORLD GENERATION - which should be its own class - consider refactor.
    public float PerlinNoiseScale = 1f;
    public float MaximumWorldHeight = 1f;
    public bool DiscreteElevationIncrements = false;
    public float UnitElevationMagnitude = 1f;
    public bool ConsiderTileChirality = false;
    public int ChunkSize = 5;
    public int InitialNumChunks = 5;
    public int ActiveChunkRadius = 5;
    public float PerlinXOffset { private set; get; }
    public float perlinZOffset { private set; get; }
    public int TileSize = 1;



    // Cached References.
    public CameraController CameraController { private set; get; }
    public TileAccessor TileAccessor { private set; get; }




    private void Awake()
    {
        CameraController = Camera.main.GetComponent<CameraController>();
        TileAccessor = GetComponent<TileAccessor>();
        DetermineNoisemapOffset(); 
        GenerateInitialChunks();
    }

    private void Update()
    {
        // Respawn map if SPACEBAR is hit:
        DEBUG_ONLY_Respawn_Map();

        // Toggle grid with G:
        ToggleTileGrid();
    }

    private void ToggleTileGrid()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            foreach (Chunk chunk in ActiveChunks)
            {
                if (chunk.ChunkTileGrid.gameObject.activeSelf)
                {
                    chunk.ChunkTileGrid.gameObject.SetActive(false);
                }
                else
                {
                    chunk.ChunkTileGrid.gameObject.SetActive(true);
                }
            }
        }
    }

    private void DEBUG_ONLY_Respawn_Map()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CameraController.InitializeCameraSettings();
            NukeTheWorld();
            DetermineNoisemapOffset();
            GenerateInitialChunks();
        }
    }

    private void DetermineNoisemapOffset()
    {
        PerlinXOffset = Random.Range(1000f, 9999f);
        perlinZOffset = Random.Range(1000f, 9999f);
    }

    private void GenerateInitialChunks()
    {
        ActiveChunks = new List<Chunk>();

        for (int x = 0; x < InitialNumChunks; x++)
        {
            for (int z = 0; z < InitialNumChunks; z++)
            {
                float xPosition = ((x * ChunkSize) - ((InitialNumChunks / 2) * ChunkSize)) * TileSize;
                float zPosition = ((z * ChunkSize) - ((InitialNumChunks / 2) * ChunkSize)) * TileSize;
                Vector3 chunkPosition = new Vector3(xPosition, 0, zPosition);
                GenerateChunk(chunkPosition);
            }
        }
    }

    private void GenerateChunk(Vector3 ChunkWorldPosition)
    {
        Chunk chunk = Instantiate(ChunkPrefab, transform.TransformPoint(ChunkWorldPosition), Quaternion.identity, transform);
        chunk.name = "Chunk: " + ChunkWorldPosition.x + ", " + ChunkWorldPosition.z + ".";
        //chunk.transform.localPosition = Vector3.zero;

        ActiveChunks.Add(chunk);
    }

    // This deletes all chunks... really bad idea. Make sure this garbage gets deleted post debugging.
    private void NukeTheWorld()
    {
        foreach(Chunk chunk in ActiveChunks)
        {
            Destroy(chunk.gameObject);
        }
    }

    // The following four functions receive notifications from the Camera Controller via UnityEvent when the edge of the map is approached. This signals the world to generate new chunks and addd them to the map.
    public void CallGenerateChunksToLeft()
    {
        GenerateNewChunks(Direction.Left);
    }

    public void CallGenerateChunksToRight()
    {
        GenerateNewChunks(Direction.Right);
    }

    public void CallGenerateChunksToTop()
    {
        GenerateNewChunks(Direction.Up);
    }

    public void CallGenerateChunksToBottom()
    {
        GenerateNewChunks(Direction.Down);
    }

    public void GenerateNewChunks(Direction directionToGenerateChunks)
    {
        switch (directionToGenerateChunks)
        {
            case Direction.Left:

                break;
            case Direction.Right:

                break;
            case Direction.Up:

                break;
            case Direction.Down:

                break;
            default:
                Debug.LogError("Error: Unhandled case in World.GenerateNewChunks()");
                break;
        }
    }
}
