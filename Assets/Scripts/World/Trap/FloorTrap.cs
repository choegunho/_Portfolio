using UnityEngine;

public class FloorTrap : MonoBehaviour
{
    private float _damage = 9999.9f;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") == true)
        {
            if(other.TryGetComponent<GetDamage>(out var damaged))
            {
                damaged.GetDamage(_damage);
            }
        }
    }
}
