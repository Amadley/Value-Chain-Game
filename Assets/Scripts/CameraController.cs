using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CameraController : MonoBehaviour
{
    public bool AllowSmoothRotation = false;
    public float RotationDegreesIncrement = 90f;
    public float TranslateSpeed = .5f;
    public float ScrollSpeed = .5f;
    public float MaximumOrbitSpeed = .5f;
    public float ZoomInUnitsAboveTerrain = 10f;
    public float ZoomOutUnitsAboveTerrain = 100f;
    [SerializeField]
    private Vector3 StartingPosition;
    [SerializeField]
    private Vector3 StartingEulerAngleRotation;

    private float MaximumZoomIn;
    private float MaximumZoomOut;
    private float targetZoom;
    [SerializeField]
    private float ZoomSmoothnessFactor = 0.3f;
    [SerializeField]
    private float zoomVelocity;
    private Vector3 CenterViewportToWorldCoordinates;

    // Cache a reference to the camera this is attached to.
    public Camera Camera { set; get; }

    // Cache a reference to the World.
    private World world;

    private void Awake()
    {
        InitializeCameraSettings();
    }

    // Update is called once per frame
    void Update()
    {
        TranslateCamera();
        zoomCamera();
        OrbitCamera();
        UpdateCenterViewportToWorldCoorindates();
    }

    public void InitializeCameraSettings()
    {
        world = FindObjectOfType<World>();
        Camera = Camera.main;

        ResetPositionAndRotation();
        CalculateZoomLimits();
        zoomVelocity = 0f;
        targetZoom = transform.position.y;
    }

    private void UpdateCenterViewportToWorldCoorindates()
    {
        Ray CenterWorldCoordinateDetector = Camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;
        if (Physics.Raycast(CenterWorldCoordinateDetector, out hit))
        {
            CenterViewportToWorldCoordinates = hit.point;
        }
    }

    private void ResetPositionAndRotation()
    {
        float cameraHeight = StartingPosition.y * world.TileSize;
        transform.position = new Vector3(StartingPosition.x, cameraHeight, StartingPosition.z);
        Quaternion rotation = Quaternion.Euler(StartingEulerAngleRotation);
        transform.rotation = rotation;
    }

    private void CalculateZoomLimits()
    {
        MaximumZoomIn = world.MaximumWorldHeight + ZoomInUnitsAboveTerrain;
        MaximumZoomOut = world.MaximumWorldHeight + (ZoomOutUnitsAboveTerrain * world.TileSize);
    }

    private void TranslateCamera()
    {
        float horizontalMovementMagnitude = Input.GetAxis("Horizontal") * TranslateSpeed;
        float verticalMovementMagnitude = Input.GetAxis("Vertical") * TranslateSpeed;

        // Set magnitude of camera movement vectors based on player input. Allow two degrees of translation freedom.
        Vector3 horizontalMovement = Vector3.right * horizontalMovementMagnitude;
        Vector3 verticalMovement = Vector3.forward * verticalMovementMagnitude;

        // Account for rotation about the y-axis.
        Quaternion yAxisRotation = Quaternion.identity;
        yAxisRotation.eulerAngles = new Vector3(0, transform.rotation.eulerAngles.y, 0);
        verticalMovement = yAxisRotation * verticalMovement;
        horizontalMovement = yAxisRotation * horizontalMovement;

        // Add horizontal and vertical input vectors to produce single translation vector.
        transform.Translate(horizontalMovement + verticalMovement, Space.World);

        // TODO - Implement clamping of the camera transform.position if the CenterOfViewport is not hitting a collider (not aligned with map).
    }

    // Naive implementation - eventual functionality to allow zoom to certain units above collider, not just certain units above max terrain level.
    private void zoomCamera()
    {
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");

        targetZoom -= scrollInput * ScrollSpeed;
        targetZoom = Mathf.Clamp(targetZoom, MaximumZoomIn, MaximumZoomOut);
        float newPosition = Mathf.SmoothDamp(transform.position.y, targetZoom, ref zoomVelocity, ZoomSmoothnessFactor);
        transform.position = new Vector3(transform .position.x, newPosition, transform.position.z);
    }

    // There is a bug - if the middle of the viewport is not aligned with a chunkmesh, rotation will be wonky (but funny!). Will have to limit the camera's motion so it cannot
    // Move outside what has been revealed in the world - this will fix this particular bug. Should also add an else case for if no hit occurs in this method.
    private void OrbitCamera()
    {
        if (AllowSmoothRotation)
        {
            if (Input.GetKey(KeyCode.Q))
            {
                transform.RotateAround(CenterViewportToWorldCoordinates, Vector3.up, -MaximumOrbitSpeed * Time.deltaTime);
            }

            if (Input.GetKey(KeyCode.E))
            {
                transform.RotateAround(CenterViewportToWorldCoordinates, Vector3.up, +MaximumOrbitSpeed * Time.deltaTime);
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {

                transform.RotateAround(CenterViewportToWorldCoordinates, Vector3.up, RotationDegreesIncrement);

            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                transform.RotateAround(CenterViewportToWorldCoordinates, Vector3.up, -RotationDegreesIncrement);
            }
        }
    }
}
