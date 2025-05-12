using UnityEngine;

public class BulletDodgingEnemyController :  MonoBehaviour
{
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float rotationSpeed = 100f;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private PlayerFinder playerFinder;

    [Header("Dodge Settings")]
    [SerializeField] private float bulletDetectionRadius = 5f;
    [SerializeField] private LayerMask bulletLayer;
    [SerializeField] private float maxTimeToImpact = 1.5f;
    [SerializeField] private float dangerDistance = 0.5f;
    [SerializeField] private float dodgeStrength = 2f;
    [SerializeField] private float wallCheckDistance = 1f;

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
        if (GameManager.Instance.GamePaused) return;

        if (_player && playerFinder.CanSeePlayer())
        {
            Vector2 directionToPlayer = (_player.position - transform.position).normalized;
            var dodgeInfluence  = Vector2.zero;

            var bulletsInRange =
                Physics2D.OverlapCircleAll(transform.position, bulletDetectionRadius, bulletLayer);
            foreach (var bulletCollider in bulletsInRange)
            {
                var bullet = bulletCollider.GetComponent<Bullet>();
                if (!bullet || !bullet.isActiveAndEnabled) continue;

                Vector2 bulletPos = bullet.transform.position;
                var toEnemy = (Vector2)transform.position - bulletPos;
                var bulletVel = bullet.Direction * bullet.Speed;

                var t = Vector2.Dot(toEnemy, bulletVel) / bulletVel.sqrMagnitude;
                if (t > 0 && t < maxTimeToImpact)
                {
                    var closestPoint = bulletPos + bulletVel * t;
                    var distanceAtClosest = Vector2.Distance(closestPoint, transform.position);
                    if (distanceAtClosest < dangerDistance)
                    {
                        var bulletDir = bullet.Direction;
                        var cross = bulletDir.x * toEnemy.y - bulletDir.y * toEnemy.x;
                        var dodgeDir = cross > 0
                            ? new Vector2(-bulletDir.y, bulletDir.x)
                            : new Vector2(bulletDir.y, -bulletDir.x);
                        dodgeDir.Normalize();

                        dodgeInfluence += dodgeDir * dodgeStrength;
                    }
                }
            }

            var finalDirection = (directionToPlayer + dodgeInfluence).normalized;
            finalDirection = GetWallAwareDirection(finalDirection, directionToPlayer);
            _movement = SnapTo45DegreeAngle(finalDirection);
        }
        else
        {
            _movement = Vector2.zero;
        }
    }

    private Vector2 GetWallAwareDirection(Vector2 desiredDirection, Vector2 fallbackDirection)
    {
        // Probe in multiple directions to find best path
        var hit = Physics2D.Raycast(transform.position, desiredDirection, wallCheckDistance, wallLayer);
        if (!hit)
            return desiredDirection;

        // Calculate wall tangent directions
        var wallNormal = hit.normal;
        var wallTangent = Vector2.Perpendicular(wallNormal).normalized;
        
        // Choose tangent direction that best aligns with desired movement
        var dot = Vector2.Dot(wallTangent, desiredDirection);
        var bestTangent = dot > 0 ? wallTangent : -wallTangent;

        // Add some base direction influence to prevent wall hugging
        return (bestTangent + fallbackDirection * 0.3f).normalized;
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