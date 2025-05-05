using UnityEngine;

public class PlayerFinder : MonoBehaviour
{
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private Transform player;
    [SerializeField] private Transform enemy;
    
    public bool CanSeePlayer()
    {
        var direction = player.position - enemy.position;
        var distance = direction.magnitude;

        var hit = Physics2D.Raycast(
            enemy.position, 
            direction.normalized, 
            distance, 
            wallLayer
        );

        return hit.collider is null; // No wall = player is visible
    }
}
