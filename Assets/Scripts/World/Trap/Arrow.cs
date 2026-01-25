using UnityEngine;

public class Arrow : MonoBehaviour
{
    private Transform _player;
    private float _damage = 30.0f;
    private float _time = 0.0f;
    private float _moveSpeed = 5.0f;
    private float _laptime = 3.5f;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") == true)
        {
            if(other.TryGetComponent<GetDamage>(out var damaged)){

                damaged.GetDamage(_damage);
                Destroy(this.gameObject);
            }
        }
    }

    private void Update()
    {
        _time += Time.deltaTime;

        transform.position += -transform.forward * Time.deltaTime * _moveSpeed;

        if (_time >= _laptime)
        {
            Destroy(this.gameObject);
        }
    }
}
