using UnityEngine;

public class Heart : MonoBehaviour
{
    [SerializeField] private PlayerStateController _player;
    [SerializeField] private float _heal = 25.0f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") == true)
        {
            _player.CurrentHealth += _heal;
            _player.CurrentHealth = Mathf.Min(_player.CurrentHealth, _player.Health);
            _player.UpdateUI();
            Destroy(gameObject);
        }   
    }
}
