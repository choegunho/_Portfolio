using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{

    private Vector3 _dir;
    public float _damage;
    private float _speed;
    private float _elapsedTime = 0.0f;
    private float _lapTime = 3.0f;
    public void Init(float damage, Vector3 dir, float speed)
    {
        _damage = damage;
        _dir = dir;
        _speed = speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Monster")) return;
        if (other.TryGetComponent<GetDamage>(out var damage))
        {
            damage.GetDamage(_damage);
            Destroy(this.gameObject);
        }
        else if (other.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            Destroy(this.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += _dir * _speed * Time.deltaTime;
        _elapsedTime += Time.deltaTime;
        if(_elapsedTime > _lapTime)
        {
            Destroy(this.gameObject);
            Debug.Log("Destroy projectile");
        }
    }
}
