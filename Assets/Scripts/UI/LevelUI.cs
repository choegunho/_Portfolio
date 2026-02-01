using UnityEngine.UI;
using UnityEngine;

public class LevelUI : MonoBehaviour
{
    [SerializeField] private Text _text;

    public void Init()
    {
        _text.text = "Lv 1";
    }

    public void LevelUp(float level)
    {
        Debug.Log("Level Up!");
        _text.text = $"Lv {level}";
    }
}
