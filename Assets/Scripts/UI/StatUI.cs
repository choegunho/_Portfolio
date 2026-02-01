using UnityEngine;
using UnityEngine.UI;

enum Type
{
    Damage,
    Defense,
    Speed
}

public class StatUI : MonoBehaviour
{
    [SerializeField] private PlayerStateController _player;
    [SerializeField] private Text _text;
    [SerializeField] private Type _type;

    public void UpdateUI()
    {
        if (_type == Type.Damage)
        {
            _text.text = $"{_player.Damage:F1}";
        }
        else if (_type == Type.Defense)
        {
            _text.text = $"{_player.Defense:F1}";
        }
        else
        {
            _text.text = $"{_player.Speed:F1}";
        }
    }
}
