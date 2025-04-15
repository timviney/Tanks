using System.Diagnostics;
using System.Threading;
using UnityEngine;

public class PlayerTurretController : MonoBehaviour 
{
    [SerializeField] private BulletPool bulletPool;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float shootCooldown = 1000f;

    private Camera _mainCam;
    private Stopwatch _sw = new();

    void Start() 
    {
        _mainCam = Camera.main;
    }

    void Update() 
    {
        AimTurret();

        if (!Input.GetMouseButton(0) || 
            (_sw.IsRunning && _sw.ElapsedMilliseconds < shootCooldown)) return;
        
        Shoot();
        _sw.Restart();
    }

    void AimTurret() 
    {
        var mousePos = _mainCam.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        var direction = mousePos - transform.position;
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