using UnityEngine;

public class ArrowTrap : MonoBehaviour
{
    [SerializeField] private GameObject _arrowPrefab;
    private float _trapCoolDown = 5.0f;
    private bool _trapCool = true;
    private float _trapTime;

    private void TrapCool()
    {
        if(Time.time >= _trapCoolDown + _trapTime)
        {
            _trapCool = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") == true && _trapCool)
        {
            _trapTime = Time.time;
            _trapCool = false;
            Instantiate(_arrowPrefab, transform.position, transform.rotation);
        } 
    }

    private void Update()
    {
        TrapCool();
    }
}
