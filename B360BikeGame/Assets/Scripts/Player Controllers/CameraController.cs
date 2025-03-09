using UnityEngine;

public class CameraController : MonoBehaviour
{
    /// <summary>
    /// Camera Control script for bike customization, Pan around bike or bike part, Zoom in and out, 
    /// When a part is selected zooms into that part
    /// </summary>

    [Header("Target Settings")]
    [SerializeField] private Transform target;
    [SerializeField] private float defaultDistance = 10f;
    [SerializeField] private float defaultHeight = 2f;

    [Header("Orbit Settings")]
    [SerializeField] private float rotationSpeed = 100f;
    [SerializeField] private float heightChangeSpeed = 5f;
    [SerializeField] private float minHeight = -5f;
    [SerializeField] private float maxHeight = 10f;

    [Header("Zoom Settings")]
    [SerializeField] private float zoomSpeed = 5f;
    [SerializeField] private float minZoomDistance = 2f;
    [SerializeField] private float maxZoomDistance = 15f;
    [SerializeField] private Transform zoomTarget;
    [SerializeField] private float zoomInDistance = 3f;

    [Header("Zoom Pan Settings")]
    [SerializeField] private float zoomedPanSpeed = 2f;
    [SerializeField] private float maxPanDistance = 5f;
    // Private variables
    private float currentRotationY;
    private float currentHeight;
    private float currentDistance;
    private Vector3 defaultPosition;
    private bool isZoomedIn = false;
    private Vector3 zoomedInOriginalPosition;
    private Vector3 currentPanOffset = Vector3.zero;


    private void Start()
    {
        if (target == null)
        {
            Debug.LogError("Please assign a target for the camera to orbit around!");
            enabled = false;
            return;
        }

        // Initialize starting position
        currentRotationY = transform.eulerAngles.y;
        currentHeight = defaultHeight;
        currentDistance = defaultDistance;

        // Apply initial camera position
        UpdateCameraPosition();

        // Store default position for resetting
        defaultPosition = transform.position;
    }

    private void Update()
    {


        if (isZoomedIn)
        {
            HandleZoomedPanning();
        }
        else
        {
            HandleRotation();
            HandleHeight();
            HandleZoom();
            UpdateCameraPosition();
        }
    }

    private void HandleRotation()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        currentRotationY -= horizontalInput * rotationSpeed * Time.deltaTime;
    }
    private void HandleHeight()
    {
        float verticalInput = Input.GetAxis("Vertical");
        currentHeight += verticalInput * heightChangeSpeed * Time.deltaTime;
        currentHeight = Mathf.Clamp(currentHeight, minHeight, maxHeight);
    }
    private void HandleZoom()
    {
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        currentDistance -= scrollInput * zoomSpeed;
        currentDistance = Mathf.Clamp(currentDistance, minZoomDistance, maxZoomDistance);
    }
    private void HandleZoomedPanning()
    {
        // Get input for panning
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Calculate camera's right and up vectors
        Vector3 right = transform.right;
        Vector3 up = transform.up;

        // Calculate pan offsets
        Vector3 horizontalPan = right * horizontalInput * zoomedPanSpeed * Time.deltaTime;
        Vector3 verticalPan = up * verticalInput * zoomedPanSpeed * Time.deltaTime;
        // Apply the pan
        currentPanOffset += horizontalPan + verticalPan;

        // Limit maximum pan distance
        if (currentPanOffset.magnitude > maxPanDistance)
        {
            currentPanOffset = currentPanOffset.normalized * maxPanDistance;
        }

        // Apply the panned position
        transform.position = zoomedInOriginalPosition + currentPanOffset;

        // Keep looking at zoom target
        transform.LookAt(zoomTarget);
    }
    private void UpdateCameraPosition()
    {
        // Calculate new position based on orbit angle and distance
        float radians = currentRotationY * Mathf.Deg2Rad;
        float x = target.position.x + currentDistance * Mathf.Sin(radians);
        float z = target.position.z + currentDistance * Mathf.Cos(radians);

        Vector3 newPosition = new Vector3(x, target.position.y + currentHeight, z);
        transform.position = newPosition;

        // Look at target
        transform.LookAt(target);
    }

    public void SetZoomTarget(Transform zTarget)
    {
        zoomTarget = zTarget;
        isZoomedIn = true;
        currentPanOffset = Vector3.zero; // Reset pan offset when entering zoom mode

        ZoomToTarget();
    }
    private void ZoomToTarget()
    {
        if (zoomTarget == null) return;

        // Calculate direction from target to camera
        Vector3 directionToCamera = (transform.position - zoomTarget.position).normalized;

        // Position camera at zoom distance
        transform.position = zoomTarget.position + directionToCamera * zoomInDistance;
        // Store this position as the original zoomed position (before panning)
        zoomedInOriginalPosition = transform.position;
        // Look at zoom target
        transform.LookAt(zoomTarget);
    }

    public void ReturnToDefaultPosition()
    {
        isZoomedIn = false;

        UpdateCameraPosition();
    }
}
