using UnityEngine;
using UnityEngine.Serialization;

public class CameraFollow : MonoBehaviour
{
    [Header("References")]
    public Transform target;
    public SpriteRenderer mapRenderer;
    
    [Header("Settings")]
    public float smoothing = 5f;
    [Range(0, 1)] 
    public float edgeThreshold = 0.8f;

    private Camera _cam;
    private float _cameraHalfWidth;
    private float _cameraHalfHeight;
    private Vector2 _mapMin;
    private Vector2 _mapMax;
    private Vector3 _initialOffset;

    private CameraAspectRatio _aspectRatioController;

    void Start()
    {
        _cam = GetComponent<Camera>();
        _aspectRatioController = _cam.gameObject.GetComponent<CameraAspectRatio>();
        _initialOffset = transform.position - target.position;
        CalculateCameraBounds();
        
        _aspectRatioController.OnAspectRatioChanged += CalculateCameraBounds; // Aspect Ratio recalcs after we Start here
    }

    void CalculateCameraBounds()
    {
        if (!mapRenderer)
        {
            Debug.LogError("Map Renderer not assigned!");
            return;
        }

        _cameraHalfWidth = _cam.orthographicSize * _cam.aspect;
        _cameraHalfHeight = _cam.orthographicSize;

        var mapBounds = mapRenderer.bounds;
        _mapMin = new Vector2(
            mapBounds.min.x + _cameraHalfWidth,
            mapBounds.min.y + _cameraHalfHeight);
        _mapMax = new Vector2(
            mapBounds.max.x - _cameraHalfWidth,
            mapBounds.max.y - _cameraHalfHeight);
    }

    void LateUpdate()
    {
        if (!target) return;
        
        var targetPosition = CalculateTargetPosition();
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothing * Time.deltaTime);
    }

    Vector3 CalculateTargetPosition()
    {
        // Start with player's position plus initial offset
        var targetPos = target.position + _initialOffset;
        
        // Calculate how close we are to map edges
        var horizontalRatio = Mathf.InverseLerp(_mapMin.x, _mapMax.x, target.position.x);
        var verticalRatio = Mathf.InverseLerp(_mapMin.y, _mapMax.y, target.position.y);
        
        // Adjust X position based on edge proximity
        if (horizontalRatio < 0.5f) // Left side of map
        {
            var leftPull = Mathf.InverseLerp(0, edgeThreshold, horizontalRatio * 2);
            targetPos.x = Mathf.Lerp(_mapMin.x, target.position.x, leftPull);
        }
        else // Right side of map
        {
            var rightPull = Mathf.InverseLerp(1, edgeThreshold, (horizontalRatio - 0.5f) * 2);
            targetPos.x = Mathf.Lerp(_mapMax.x, target.position.x, rightPull);
        }
        
        // Adjust Y position based on edge proximity
        if (verticalRatio < 0.5f) // Bottom side of map
        {
            var bottomPull = Mathf.InverseLerp(0, edgeThreshold, verticalRatio * 2);
            targetPos.y = Mathf.Lerp(_mapMin.y, target.position.y, bottomPull);
        }
        else // Top side of map
        {
            var topPull = Mathf.InverseLerp(1, edgeThreshold, (verticalRatio - 0.5f) * 2);
            targetPos.y = Mathf.Lerp(_mapMax.y, target.position.y, topPull);
        }
        
        // Maintain camera Z position
        targetPos.z = transform.position.z;
        
        return targetPos;
    }
}