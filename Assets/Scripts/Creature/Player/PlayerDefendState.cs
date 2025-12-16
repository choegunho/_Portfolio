using UnityEditorInternal;
using UnityEngine;

public class PlayerDefendState : IState
{
    private PlayerStateController _player;

    public PlayerDefendState(PlayerStateController player)
    {
        _player = player;
    }

    public void Enter()
    {
        _player.Animator.SetBool("Defend", true);
    }

    public void Execute()
    {
        if (Input.GetMouseButtonUp(1))
        {
            Vector2 input = _player.GetMoveInput();

            if (input.magnitude < 1.0f) // 움직임이 없으면
            {
                _player.StateMachine.ChangeState(_player.IdleState);
            }
            else    // 움직임이 있으면
            {
                _player.StateMachine.ChangeState(_player.MoveState);
            }
        }
    }

    public void Exit()
    {
        _player.Animator.SetBool("Defend", false);
    }
}
