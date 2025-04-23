using System.Threading.Tasks;
using UnityEngine;

public class TankHealth : MonoBehaviour
{
    [SerializeField] private GameObject explosionPrefab;
    public void TakeDamage(float damage)
    {
        Explode();
        GameManager.Instance.RegisterDeath(gameObject);
    }
    
    private void Explode() {
        if (explosionPrefab != null) {
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
    }
}
