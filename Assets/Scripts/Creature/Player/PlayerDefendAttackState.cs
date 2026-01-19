using UnityEngine;

public class PlayerDefendAttackState : IState
{
    private PlayerStateController _player;
    private float _enterTime;

    public PlayerDefendAttackState(PlayerStateController player)
    {
        _player = player;
    }
    public void Enter()
    {
        _enterTime = Time.time;
        _player.Animator.SetTrigger("DefendAttack");
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

        // Safety net: if we didn't enter Attack02 shortly after triggering, don't get stuck in this state.
        // (Can happen if animator state name differs, transitions change, or trigger is consumed unexpectedly.)
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
