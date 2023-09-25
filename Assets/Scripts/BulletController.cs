
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float speed = 1f;
    public float timeToLive = 1f;
    public float damage = 1f;
    private float _lifeSpan = 0;
    private GameObject _shooter;
    private Vector2 _trajectory;

    private void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject == _shooter || col.tag == "Projectile") return;
        print("triggered enter");
        Destroy(gameObject);
    }

    // Update is called once per frame
    private void Update()
    {
        transform.Translate(_trajectory * Time.deltaTime * speed);
        _lifeSpan += Time.deltaTime;
        if (_lifeSpan > timeToLive) {
            Destroy(gameObject);
        }
    }

    public void Setup(GameObject shooter, Vector2 trajectory) {
        _shooter = shooter;
        _trajectory = trajectory;
    }

    public void SetTrajectory(Vector2 trajectory) {
        _trajectory = trajectory;
    }

    public void SetShooter(GameObject shooter) {
        _shooter = shooter;
    }
}
