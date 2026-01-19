using UnityEngine;

public class PlayerAttackState : IState
{
    private PlayerStateController _player;
    private bool _isFinished;

    public PlayerAttackState(PlayerStateController player)
    {
        _player = player;
    }

    public void Enter()
    {
        _isFinished = false;
        _player.Animator.SetTrigger("Attack");
    }

    public void Execute()
    {
        if (_isFinished)
            return;

        var stateInfo = _player.Animator.GetCurrentAnimatorStateInfo(0);

        // 애니메이션 끝나면 상태 전환
        if (stateInfo.IsName("Attack01") && stateInfo.normalizedTime >= 0.75f)
        {
            _isFinished = true;

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
