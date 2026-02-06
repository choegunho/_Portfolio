using UnityEngine;
using UnityEngine.UI;

public class SkillCoolDown : MonoBehaviour
{
    [SerializeField] Text _slashSkillCoolDownText;
    [SerializeField] Text _projectileSkillCoolDownText;
    [SerializeField] PlayerStateController _player;
    private float _skillCool;

    private void Start()
    {
        _slashSkillCoolDownText.enabled = false;
        _projectileSkillCoolDownText.enabled = false;
    }

    public void SlashSkillCoolDown()
    {
        float remain = _player.GetRemainSkillCoolTime();

        if(remain > 0.0f)
        {
            _slashSkillCoolDownText.enabled = true;
            _slashSkillCoolDownText.text = remain.ToString("F1");
        }
        else
        {
            _slashSkillCoolDownText.enabled = false;
        }
    }

    public void ProjectileSkillCoolDown()
    {
        float remain = _player.GetRemainProjectileSkillCoolTime();

        if (remain > 0.0f)
        {
            _projectileSkillCoolDownText.enabled = true;
            _projectileSkillCoolDownText.text = remain.ToString("F1");
        }
        else
        {
            _projectileSkillCoolDownText.enabled = false;
        }
    }

    public void Update()
    {
        SlashSkillCoolDown();
        ProjectileSkillCoolDown();
    }
}
