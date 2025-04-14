using UnityEngine;
using System.Collections.Generic;

public class BulletPool : MonoBehaviour {
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private int initialPoolSize = 20;
    
    private readonly Stack<GameObject> _inactiveBullets = new();
    private readonly List<GameObject> _activeBullets = new();

    void Start() 
    {
        // Pre-instantiate bullets
        for (int i = 0; i < initialPoolSize; i++) {
            var bullet = Instantiate(bulletPrefab, transform);
            bullet.SetActive(false);
            _inactiveBullets.Push(bullet);
        }
    }

    public GameObject GetBullet() 
    {
        if (_inactiveBullets.Count == 0)
        {
            var activeBullet = _activeBullets[0];
            ReturnBullet(activeBullet);
            return activeBullet;
        }
        
        var bullet = _inactiveBullets.Pop();
        bullet.SetActive(true);
        _activeBullets.Add(bullet);
        return bullet;
    }

    public void ReturnBullet(GameObject bullet) 
    {
        bullet.SetActive(false);
        _activeBullets.Remove(bullet);
        _inactiveBullets.Push(bullet);
    }
}