using UnityEngine;

public class PlayerDeadState : IState
{
    private PlayerStateController _player;

    public PlayerDeadState(PlayerStateController player)
    {
        _player = player;
    }
    public void Enter()
    {
        _player.Animator.SetTrigger("Dead");
        Debug.Log("Player is Dead");

        _player.MenuChangeCount();
    }

    public void Execute()
    {

    }

    public void Exit()
    {

    }
}
