using UnityEngine;

public class PlayerIdleState : IState
{
    private PlayerStateController _player;

    public PlayerIdleState(PlayerStateController player)
    {
        _player = player;
    }
    public void Enter()
    {

    }

    public void Execute()
    {
        Vector2 input = _player.GetMoveInput();
        if(input.magnitude > 0.1)
        {
            _player.StateMachine.ChangeState(_player.MoveState);
        }

        _player.Attack();
        _player.SkillAttack();
        _player.Defend();
        if (_player.IsDead())
        {
            _player.StateMachine.ChangeState(_player.DeadState);
        }
    }

    public void Exit()
    {

    }
}
