using UnityEngine;
using UnityEngine.UI;

public class SkillCoolDown : MonoBehaviour
{
    [SerializeField] Text _coolDownText;
    [SerializeField] PlayerStateController _player;
    private float _skillCool;

    private void Start()
    {
        _coolDownText.enabled = false;
    }

    public void CoolDown()
    {
        float remain = _player.GetRemainSkillCoolTime();

        if(remain > 0.0f)
        {
            _coolDownText.enabled = true;
            _coolDownText.text = remain.ToString("F1");
        }
        else
        {
            _coolDownText.enabled = false;
        }
    }

    public void Update()
    {
        CoolDown();
    }
}
