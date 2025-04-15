using System;
using UnityEngine;
using System.Collections.Generic;

public class BulletPool : MonoBehaviour {
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private int initialPoolSize = 30;
    
    private readonly Stack<BulletInstance> _inactiveBullets = new();
    private readonly List<BulletInstance> _activeBullets = new();

    void Start() 
    {
        // Pre-instantiate bullets
        for (int i = 0; i < initialPoolSize; i++) {
            var bullet = Instantiate(bulletPrefab, transform);
            bullet.SetActive(false);
            _inactiveBullets.Push(new BulletInstance
            {
                gameObject = bullet, 
                component = bullet.GetComponent<Bullet>()
            });
        }
    }

    public BulletInstance GetBullet() 
    {
        if (_inactiveBullets.Count == 0)
        {
            var activeBullet = _activeBullets[0];
            ReturnBullet(activeBullet);
            return activeBullet;
        }
        
        var bulletInstance = _inactiveBullets.Pop();
        bulletInstance.gameObject.SetActive(true);
        _activeBullets.Add(bulletInstance);
        return bulletInstance;
    }

    public void ReturnBullet(BulletInstance bullet) 
    {
        bullet.gameObject.SetActive(false);
        _activeBullets.Remove(bullet);
        _inactiveBullets.Push(bullet);
    }
}

public struct BulletInstance : IEquatable<BulletInstance>
{
    public GameObject gameObject;
    public Bullet component;

    public BulletInstance(GameObject go, Bullet bullet)
    {
        gameObject = go;
        component = bullet;
    }

    public bool Equals(BulletInstance other)
    {
        return gameObject == other.gameObject;
    }

    public override bool Equals(object obj)
    {
        return obj is BulletInstance other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(gameObject);
    }
}