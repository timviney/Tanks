using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.Events;

[RequireComponent(typeof(Tilemap))]
[RequireComponent(typeof(CompositeCollider2D))]
public class TilemapButton : MonoBehaviour
{
    [Header("Visual Settings")]
    [SerializeField] private Color normalColor = Color.white;
    [SerializeField] private Color hoverColor = new Color(0.8f, 0.8f, 0.8f);
    [SerializeField] private Color clickColor = new Color(0.6f, 0.6f, 0.6f);
    [SerializeField] private float colorChangeSpeed = 15f;

    [Header("Click Event")]
    public UnityEvent onClick;

    private Tilemap _tilemap;
    private CompositeCollider2D _col;
    private Camera _mainCamera;
    private Color _targetColor;
    private bool _mouseIsOver;
    private bool _mouseWasPressedHere;

    void Awake()
    {
        _tilemap = GetComponent<Tilemap>();
        _col = GetComponent<CompositeCollider2D>();
        _mainCamera = Camera.main;

        _col.isTrigger = true;

        _targetColor = normalColor;
    }

    void Update()
    {
        CheckMouseState();
        UpdateVisuals();
    }

    void CheckMouseState()
    {
        var mouseWorldPos = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
        var currentlyOver = _col.OverlapPoint(mouseWorldPos);

        // State changes
        if (currentlyOver != _mouseIsOver)
        {
            _mouseIsOver = currentlyOver;
            _targetColor = _mouseIsOver ? hoverColor : normalColor;
        }

        // Click handling
        if (_mouseIsOver)
        {
            if (Input.GetMouseButtonDown(0))
            {
                _mouseWasPressedHere = true;
                _targetColor = clickColor;
            }

            if (_mouseWasPressedHere && Input.GetMouseButtonUp(0))
            {
                onClick.Invoke();
                _targetColor = hoverColor;
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            _mouseWasPressedHere = false;
            if (!_mouseIsOver) _targetColor = normalColor;
        }
    }

    void UpdateVisuals()
    {
        if (_tilemap.color != _targetColor)
        {
            _tilemap.color = Color.Lerp(_tilemap.color, _targetColor, Time.deltaTime * colorChangeSpeed);
        }
    }

    void OnDisable()
    {
        // Reset state when disabled
        _tilemap.color = normalColor;
        _mouseIsOver = false;
        _mouseWasPressedHere = false;
    }
}