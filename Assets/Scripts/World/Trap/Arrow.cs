using UnityEngine;

public class Arrow : MonoBehaviour
{
    private Transform _player;
    private float _damage = 30.0f;
    private float _moveSpeed = 5.0f;
    private float _lapTime;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") == true)
        {
            if(other.TryGetComponent<GetDamage>(out var damaged)){

                damaged.GetDamage(_damage);
                Destroy(this.gameObject);
            }
        }
        else if(other.gameObject.layer == LayerMask.NameToLayer("Wall") && _lapTime >= 1.0f)
        {
            Destroy(this.gameObject);
        }
    }

    private void Update()
    {
        _lapTime += Time.deltaTime;
        transform.position += -transform.forward * Time.deltaTime * _moveSpeed;
    }
}
