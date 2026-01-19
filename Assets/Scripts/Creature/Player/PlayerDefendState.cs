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
        if(Input.GetMouseButton(0) && _player.CanDefendAttack())
        {
            _player.DefendAttack();
        }
        if (Input.GetMouseButtonUp(1))
        {
            Vector2 input = _player.GetMoveInput();

            if (input.magnitude < 1.0f) // �������� ������
            {
                _player.StateMachine.ChangeState(_player.IdleState);
            }
            else    // �������� ������
            {
                _player.StateMachine.ChangeState(_player.MoveState);
            }
        }
    }

    public void Exit()
    {
        _player.DisableShieldEffect();
        _player.Animator.SetBool("Defend", false);
    }
}
