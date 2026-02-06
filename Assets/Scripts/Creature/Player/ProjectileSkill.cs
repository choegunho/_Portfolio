using UnityEngine;

public class ProjectileSkill : MonoBehaviour
{
    [SerializeField] private float _speed = 6.0f;
    private float _damage;
    private float _elapsedTime = 0.0f;
    private float _laptime = 1.5f;
    private PlayerStateController _player;

    private void Awake()
    {
        _player = FindAnyObjectByType<PlayerStateController>();
    }
    public void SetDamage(float damage)
    {
        _damage = damage;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Monster") == true)
        {
            other.GetComponent<Monster>().GetDamage(_damage);
        }
    }

    private void Update()
    {
        _elapsedTime += Time.deltaTime;
        transform.position += Time.deltaTime * transform.forward * _speed;
        if(_elapsedTime >= _laptime)
        {
            Destroy(this.gameObject);
        }
    }
}
