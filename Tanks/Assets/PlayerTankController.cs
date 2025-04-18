using UnityEngine;

public class PlayerTankController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 3;
    [SerializeField] private float rotationSpeed = 100f;
    
    private Rigidbody2D _rb;
    private Vector2 _movement;
    private float _nominalRotation;

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
        
        float targetAngle; 
        if (_movement == Vector2.zero)
        {
            targetAngle = _rb.rotation;
            _rb.angularVelocity = 0f;
        }
        else targetAngle = Mathf.Atan2(_movement.y, _movement.x) * Mathf.Rad2Deg - 90f;// -90deg to face upward initially
        
        _nominalRotation = Mathf.LerpAngle(_nominalRotation, targetAngle, rotationSpeed * Time.fixedDeltaTime);
        _rb.rotation = Mathf.RoundToInt(_nominalRotation / 15f) * 15f; //Lock into 15 degrees only
    }
}