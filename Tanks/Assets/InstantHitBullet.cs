using UnityEngine;

public class InstantHitBullet : MonoBehaviour {
    [SerializeField] private float speed = 6f;
    private Vector2 _direction;

    public void SetDirection(Vector2 direction) {
        _direction = direction;
    }

    void Update() {
        // Move bullet manually (no physics for efficiency!!)
        transform.Translate(_direction * (speed * Time.deltaTime));
    }

    void OnTriggerEnter2D(Collider2D other) {
        // Let bullets pass through each other to reduce computation!
        if (other.gameObject.layer == LayerMask.NameToLayer("Bullets") || 
            other.gameObject.layer == LayerMask.NameToLayer("Player")) return;

        Destroy(gameObject); // Vanish on hit
    }
}