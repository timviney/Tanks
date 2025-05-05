using System.Diagnostics;
using UnityEngine;

public class PlayerTrackingTurret :  MonoBehaviour 
{
    [SerializeField] private BulletPool bulletPool;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float shootCooldown = 1000f;
    [SerializeField] private PlayerFinder playerFinder;

    private Transform _player;
    private Stopwatch _sw = new Stopwatch();

    void Start() 
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        _sw.Start();
    }

    void Update() 
    {
        if (!GameManager.Instance.GameStarted) return;

        if (!_player) return;

        AimTurret();

        if (IsCoolingDown() || !playerFinder.CanSeePlayer()) return;
        
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
        bullet.GameObject.transform.position = firePoint.position;
        bullet.GameObject.transform.rotation = firePoint.rotation;
        bullet.Component.SetDirection(transform.right, bulletPool);
    }
}
