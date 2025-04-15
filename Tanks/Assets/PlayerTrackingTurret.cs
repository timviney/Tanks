using System.Diagnostics;
using UnityEngine;

public class PlayerTrackingTurret :  MonoBehaviour 
{
    [SerializeField] private BulletPool bulletPool;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float shootCooldown = 1000f;
    
    private Transform _player;
    private Stopwatch _sw = new Stopwatch();

    void Start() 
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        _sw.Start();
    }

    void Update() 
    {
        if (!_player) return;

        AimTurret();

        if (IsCoolingDown()) return;
        
        Shoot();
        _sw.Restart();
    }

    private bool IsCoolingDown()
    {
        return _sw.ElapsedMilliseconds < shootCooldown;
    }

    void AimTurret() 
    {
        var direction = _player.position - transform.position;
        var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    void Shoot() 
    {
        var bullet = bulletPool.GetBullet();
        bullet.gameObject.transform.position = firePoint.position;
        bullet.gameObject.transform.rotation = firePoint.rotation;
        bullet.component.SetDirection(transform.right, bulletPool);
    }
}
