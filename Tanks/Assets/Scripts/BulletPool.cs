using System;
using UnityEngine;
using System.Collections.Generic;

public class BulletPool : MonoBehaviour {
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private int initialPoolSize = 30;
    
    private readonly Queue<BulletInstance> _inactiveBullets = new();
    private readonly List<BulletInstance> _activeBullets = new();

    void Start() 
    {
        // Pre-instantiate bullets
        for (int i = 0; i < initialPoolSize; i++) {
            var bullet = Instantiate(bulletPrefab, transform);
            bullet.SetActive(false);
            _inactiveBullets.Enqueue(new BulletInstance
            {
                GameObject = bullet, 
                Component = bullet.GetComponent<Bullet>()
            });
        }
    }

    public BulletInstance GetBullet() 
    {
        BulletInstance bullet;
        if (_inactiveBullets.Count == 0)
        {
            var activeBullet = _activeBullets[0];
            ReturnBullet(activeBullet);
            bullet = activeBullet;
        }
        else
        {
            bullet = _inactiveBullets.Dequeue();
        }
        
        bullet.GameObject.SetActive(true);
        _activeBullets.Add(bullet);
        return bullet;
    }

    public void ReturnBullet(BulletInstance bullet) 
    {
        bullet.GameObject.SetActive(false);
        _activeBullets.Remove(bullet);
        _inactiveBullets.Enqueue(bullet);
    }
}

public struct BulletInstance : IEquatable<BulletInstance>
{
    public GameObject GameObject;
    public Bullet Component;

    public BulletInstance(GameObject go, Bullet bullet)
    {
        GameObject = go;
        Component = bullet;
    }

    public bool Equals(BulletInstance other)
    {
        return GameObject == other.GameObject;
    }

    public override bool Equals(object obj)
    {
        return obj is BulletInstance other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(GameObject);
    }
}