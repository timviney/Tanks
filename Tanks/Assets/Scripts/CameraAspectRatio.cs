using UnityEngine;
using System;

public class CameraAspectRatio : MonoBehaviour
{
    public float targetAspect = 16f / 9f;
    private Camera _camera;
    public float ScaleHeight { get; private set; } = 1f;
    public float ScaleWidth { get; private set; } = 1f;
    
    public event Action OnAspectRatioChanged;

    void Start()
    {
        _camera = GetComponent<Camera>();
        UpdateAspectRatio();
    }

    void Update()
    {
        if (Screen.width == _lastWidth && Screen.height == _lastHeight) return;
        
        UpdateAspectRatio();
        _lastWidth = Screen.width;
        _lastHeight = Screen.height;
    }

    private int _lastWidth;
    private int _lastHeight;

    void UpdateAspectRatio()
    {
        var windowAspect = (float)Screen.width / Screen.height;
        var scaleHeight = windowAspect / targetAspect;

        if (scaleHeight < 1.0f) // Letterbox
        {
            ScaleHeight = scaleHeight;
            var rect = _camera.rect;
            rect.width = 1.0f;
            rect.height = scaleHeight;
            rect.x = 0;
            rect.y = (1.0f - scaleHeight) / 2.0f;
            _camera.rect = rect;
        }
        else // Pillarbox
        {
            var scaleWidth = 1.0f / scaleHeight;
            ScaleWidth = scaleWidth;
            var rect = _camera.rect;
            rect.width = scaleWidth;
            rect.height = 1.0f;
            rect.x = (1.0f - scaleWidth) / 2.0f;
            rect.y = 0;
            _camera.rect = rect;
        }
        
        OnAspectRatioChanged?.Invoke();
    }
}