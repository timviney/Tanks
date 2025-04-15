using UnityEngine;

public class PlayerTankController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 3;
    [SerializeField] private float rotationSpeed = 100f;
    
    private Rigidbody2D _rb;
    private Vector2 _movement;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Get WASD input
        _movement.x = Input.GetAxisRaw("Horizontal");
        _movement.y = Input.GetAxisRaw("Vertical");
    }

    void FixedUpdate()
    {
        // Move the tank
        _rb.linearVelocity = _movement.normalized * moveSpeed;

        // Rotate the tank toward movement direction (only if moving)
        if (_movement == Vector2.zero) return;
        
        var targetAngle = Mathf.Atan2(_movement.y, _movement.x) * Mathf.Rad2Deg - 90f; // -90° to face upward initially
        var angle = Mathf.LerpAngle(_rb.rotation, targetAngle, rotationSpeed * Time.fixedDeltaTime);
        _rb.rotation = angle;
    }
}