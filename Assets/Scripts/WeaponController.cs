using System;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
  private bool _isFiring = false;
  private float _lastFiretime = 0;
  private GameObject _shooter;
  [Range(0.1f, 1)]public float fireRate = 0.1f;
  public GameObject bulletPrefab;
  private Vector2 _aim = Vector2.right;

  private void Update() {
    if (CanFire()) {
      print("firing weapon");
      _lastFiretime = Time.time;
      GameObject bullet = Instantiate(bulletPrefab);
      BulletController controller = bullet.GetComponent<BulletController>();
      // TODO: Add variance to bullet trajectory based on _aim and weapon accuracy
      controller.Setup(_shooter, _aim);
      bullet.transform.position = transform.position;
    }
  }

  public void Setup(GameObject shooter) {
    _shooter = shooter;
    _lastFiretime = 0;
    _isFiring = false;
  }

  public void StartFiring() {
    _isFiring = true;
  }

  public void AimAt(Vector2 aim) {
    _aim = aim;
  }

  public void StopFiring() {
    _isFiring = false;
  }

  private bool CanFire() {
    return _isFiring && Time.time - _lastFiretime >= fireRate;
  }
}
