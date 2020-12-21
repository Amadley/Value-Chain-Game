using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TileAccessor : MonoBehaviour
{
    // Cached reference to the World.
    World world;

    // Cache reference to camera.
    Camera mainCamera;

    // These variables relate to player input with mouse movement.
    private Tile lastFrameTileHoveredOn;
    private Tile currentFrameTileHoveredOn;
    private bool tileHoverTimerStarted;
    private float timeTileHoverStarted;
    private bool tileHover;
    private bool mouseoverEventInvoked;

    [SerializeField]
    private float timeDelayToDisplayTooltip = 1.5f;
    public float TimeDelayToDisplayTootip
    {
        private set
        {
            timeDelayToDisplayTooltip = value;
        }
        get
        {
            return timeDelayToDisplayTooltip;
        }
    }

    void Awake()
    {
        world = GetComponent<World>();
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        DisplayTileTooltips();
    }

    // Calls mouseover event if a given tile is hovered over by mouse for greater than designated time delay. mouseover event is listened to by TileTooltip monobehaviour.
    private void DisplayTileTooltips()
    {
        currentFrameTileHoveredOn = GetTileUnderMouseCursor();

        if (currentFrameTileHoveredOn != null)
        {
            if (currentFrameTileHoveredOn == lastFrameTileHoveredOn)
            {
                if (!tileHoverTimerStarted)
                {
                    tileHoverTimerStarted = true;
                    tileHover = true;
                    timeTileHoverStarted = Time.time;
                }

                if (Time.time > timeTileHoverStarted + timeDelayToDisplayTooltip && !mouseoverEventInvoked)
                {
                    TileTooltip.Instance.ShowTooltip(GetTileUnderMouseCursor());
                    mouseoverEventInvoked = true;
                }
            }
            else
            {
                TileTooltip.Instance.HideTooltip();
                tileHoverTimerStarted = false;
                tileHover = false;
                mouseoverEventInvoked = false;
            }
        }

        lastFrameTileHoveredOn = currentFrameTileHoveredOn;
    }

    public Tile GetTileUnderMouseCursor()
    {
        Tile tile = null;

        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            Chunk chunk = hit.collider.gameObject.GetComponent<Chunk>();
            tile = GetTileAtWorldCoordinates(hit.point, chunk);
        }

        return tile;
    }

    private Tile GetTileAtWorldCoordinates(Vector3 worldCoordinatesOfTile, Chunk chunk)
    {
        Vector3 localCoordinatesOfTile = chunk.transform.InverseTransformPoint(worldCoordinatesOfTile);
        Tile tile = chunk.ChunkTiles[(int)(localCoordinatesOfTile.x / chunk.TileSize), (int)(localCoordinatesOfTile.z / chunk.TileSize)];

        return tile;
    }
}
