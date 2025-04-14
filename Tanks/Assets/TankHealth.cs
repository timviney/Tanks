using UnityEngine;

public class TankHealth : MonoBehaviour
{
    [SerializeField] private GameObject explosionPrefab;
    public void TakeDamage(float damage)
    {
        Explode();
    }
    
    private void Explode() {
        if (explosionPrefab != null) {
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
    }
}
