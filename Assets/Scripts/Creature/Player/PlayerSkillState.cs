using UnityEngine;

public class PlayerSkillState : IState
{
    private PlayerStateController _player;
    private float _enterTime;

    public PlayerSkillState(PlayerStateController player)
    {
        _player = player;
    }
    public void Enter()
    {
        _enterTime = Time.time;
        _player.Animator.SetTrigger("Skill");
    }

    public void Execute()
    {
        var animator = _player.Animator;
        var stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        // Normal path: wait until the DefendAttack animation finishes.
        if (stateInfo.IsName("Attack02"))
        {
            if (stateInfo.normalizedTime < 0.75f) return;

            Vector2 input = _player.GetMoveInput();
            if (input.magnitude < 0.1f)
                _player.StateMachine.ChangeState(_player.IdleState);
            else
                _player.StateMachine.ChangeState(_player.MoveState);

            return;
        }

        if (Time.time - _enterTime >= 0.25f && !animator.IsInTransition(0))
        {
            Vector2 input = _player.GetMoveInput();
            if (input.magnitude < 0.1f)
                _player.StateMachine.ChangeState(_player.IdleState);
            else
                _player.StateMachine.ChangeState(_player.MoveState);
        }
    }

    public void Exit()
    {

    }
}
