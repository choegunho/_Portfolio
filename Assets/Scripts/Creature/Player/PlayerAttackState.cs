using UnityEngine;

public class PlayerAttackState : IState
{
    private PlayerStateController _player;

    public PlayerAttackState(PlayerStateController player)
    {
        _player = player;
    }

    public void Enter()
    {
        _player.Animator.SetTrigger("Attack");
    }

    public void Execute()
    {
        if(_player.Animator.GetCurrentAnimatorStateInfo(0).IsName("Attack01") == true)
        {
            float animTime = _player.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime;

            if(animTime >= 1.0f)    // 애니메이션이 끝났을 때
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
            else
            {
                return;
            }
        }
    }

    public void Exit()
    {

    }
}
