using UnityEngine;

public class Bullet : MonoBehaviour 
{
    [SerializeField] private float speed = 4f;
    [SerializeField] private LayerMask collisionLayers; 
    private Vector2 _direction;
    private float _lifetime = 10f;
    private BulletPool _pool;
    
    public Vector2 Direction => _direction;
    public float Speed => speed;
    
    public void SetDirection(Vector2 direction, BulletPool pool) 
    {
        _direction = direction.normalized;
        _pool = pool;
        _lifetime = 5f;
    }

    void Update() 
    {
        if (GameManager.Instance.GamePaused) return;
        
        var distance = speed * Time.deltaTime;

        var hit = Physics2D.Raycast(transform.position, _direction, distance, collisionLayers);
        if (hit.collider is not null)
        {
            ReturnToPool();
            return;
        }
        transform.Translate(_direction * distance, Space.World);
        if ((_lifetime -= Time.deltaTime) <= 0) ReturnToPool();
    }
    
    private void ReturnToPool() 
    {
        if (_pool is not null) 
        {
            _pool.ReturnBullet(new BulletInstance(gameObject, this));
        } 
        else 
        {
            Destroy(gameObject); // Fallback
        }
    }
    
    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Bullets")) return;

        if (other.CompareTag("Player") || other.CompareTag("Enemies")) {
            var health = other.GetComponent<TankHealth>();
            if (health != null) {
                health.TakeDamage(1);
            }
        }

        ReturnToPool();
    }
}