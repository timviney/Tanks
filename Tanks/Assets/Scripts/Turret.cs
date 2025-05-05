using System.Diagnostics;
using UnityEngine;

public class Turret :  MonoBehaviour 
{
    [SerializeField] private BulletPool bulletPool;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float shootCooldown = 1000f;
    
    private Stopwatch _sw = new();

    void Start() 
    {
        if (!GameManager.Instance.GameStarted) return;

        _sw.Start();
    }
    
    private bool IsCoolingDown()
    {
        return _sw.ElapsedMilliseconds < shootCooldown;
    }
    
    void Shoot() 
    {
        var bullet = bulletPool.GetBullet();
        bullet.GameObject.transform.position = firePoint.position;
        bullet.GameObject.transform.rotation = firePoint.rotation;
        bullet.Component.SetDirection(transform.right, bulletPool);
    }
}