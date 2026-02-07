using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SkillCoolDown : MonoBehaviour
{
    [SerializeField] Text _slashSkillCoolDownText;
    [SerializeField] Text _projectileSkillCoolDownText;
    [SerializeField] Image _slashskillImage;
    [SerializeField] Image _projectileSkillImage;
    [SerializeField] PlayerStateController _player;

    private bool _activateSlashSkill = false;
    private bool _activateProjectileSkill = false;

    private void Start()
    {
        _slashskillImage.color = new Color32(70, 70, 70, 255);
        _projectileSkillImage.color = new Color32(70, 70, 70, 255);
    }

    public void ActivateSlashSkill()
    {
        _slashskillImage.color = new Color32(255, 255, 255, 255);
        _slashSkillCoolDownText.text = " ";
        _slashSkillCoolDownText.fontSize = 40;
        _slashSkillCoolDownText.color = Color.black;
        _slashSkillCoolDownText.enabled = false;
        _activateSlashSkill = true;
    }

    public void ActivateProjectileSkill()
    {
        _projectileSkillImage.color = new Color32(255, 255, 255, 255);
        _projectileSkillCoolDownText.text = " ";
        _projectileSkillCoolDownText.fontSize = 40;
        _projectileSkillCoolDownText.color = Color.black;
        _projectileSkillCoolDownText.enabled = false;
        _activateProjectileSkill = true;
    }

    private void SlashSkillCoolDown()
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

    private void ProjectileSkillCoolDown()
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
        if (_activateSlashSkill)
        {
            SlashSkillCoolDown();
        }
        if (_activateProjectileSkill)
        {
            ProjectileSkillCoolDown();
        }
    }
}
