using UnityEngine;

public class PlayerMoveState : IState
{
    private PlayerStateController _player;

    public PlayerMoveState(PlayerStateController player)
    {
        _player = player;
    }

    public void Enter()
    {
        _player.Animator.SetBool("Run", true);
    }

    public void Execute()
    {
        Vector2 input = _player.GetMoveInput();
        if(input.magnitude < 0.1f)
        {
            _player.StateMachine.ChangeState(_player.IdleState);
        }
        _player.Move(input);
        _player.Attack();
        _player.Defend();
        if (_player.IsDead())
        {
            _player.StateMachine.ChangeState(_player.DeadState);
        }
    }

    public void Exit()
    {
        _player.Animator.SetBool("Run", false);
    }
}
