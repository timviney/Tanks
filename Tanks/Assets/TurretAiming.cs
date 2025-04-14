using UnityEngine;

public class TurretAiming : MonoBehaviour {
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float bulletForce = 10f;
    private Camera _mainCam;

    void Start() {
        _mainCam = Camera.main;
    }

    void Update() {
        AimTurret();
        if (Input.GetMouseButtonDown(0)) { // Left-click to shoot
            Shoot();
        }
    }

    void AimTurret() {
        var mousePos = _mainCam.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        var direction = mousePos - transform.position;
        var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    void Shoot() {
        var bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        var bulletScript = bullet.GetComponent<InstantHitBullet>();
        bulletScript.SetDirection(firePoint.right); // Shoot toward turret's aim
    }
}