using UnityEngine;

public class EnemyController :  MonoBehaviour
{
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float rotationSpeed = 100f;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private PlayerFinder playerFinder;

    private Rigidbody2D _rb;
    private Transform _player;
    private Vector2 _movement;
    private float _nominalRotation;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (_player && playerFinder.CanSeePlayer())
        {
            var direction = _player.position - transform.position;
            direction.Normalize();
            _movement = SnapTo45DegreeAngle(direction);
        }
        else
        {
            _movement = Vector2.zero;
        }
    }

    Vector2 SnapTo45DegreeAngle(Vector2 originalDirection)
    {
        // Calculate the angle in degrees
        var angle = Mathf.Atan2(originalDirection.y, originalDirection.x) * Mathf.Rad2Deg;
    
        // Snap to nearest 45deg angle
        var snappedAngle = Mathf.Round(angle / 45f) * 45f;
    
        // Convert back to direction vector
        var radAngle = snappedAngle * Mathf.Deg2Rad;
        return new Vector2(Mathf.Cos(radAngle), Mathf.Sin(radAngle)).normalized;
    }
    
    void FixedUpdate()
    {
        _rb.linearVelocity = _movement.normalized * moveSpeed;
        
        float targetAngle; 
        if (_movement == Vector2.zero)
        {
            targetAngle = _rb.rotation;
            _rb.angularVelocity = 0;
        }
        else targetAngle = Mathf.Atan2(_movement.y, _movement.x) * Mathf.Rad2Deg - 90f;// -90deg to face upward initially
        
        _nominalRotation = Mathf.LerpAngle(_nominalRotation, targetAngle, rotationSpeed * Time.fixedDeltaTime);
        _rb.rotation = Mathf.RoundToInt(_nominalRotation / 15f) * 15f; //Lock into 15 degrees only
    }
}