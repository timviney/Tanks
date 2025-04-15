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
            _movement = direction;
        }
        else
        {
            _movement = Vector2.zero;
        }

    }

    void FixedUpdate()
    {
        _rb.linearVelocity = _movement * moveSpeed;

        if (_movement == Vector2.zero) return;
        
        var targetAngle = Mathf.Atan2(_movement.y, _movement.x) * Mathf.Rad2Deg - 90f;
        var angle = Mathf.LerpAngle(_rb.rotation, targetAngle, rotationSpeed * Time.fixedDeltaTime);
        _rb.rotation = angle;
    }
}