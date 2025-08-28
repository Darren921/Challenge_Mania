using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRunningState : PlayerMovingState
{
    protected override float moveSpeed => _player.RunSpeed;
   
   public Action sprintRefillAction;
    internal override void EnterState(PlayerStateManager playerStateManager, PlayerController player)
    {
        _smoothedMoveVelocity = Vector2.zero;
        _smoothedMoveDir = Vector2.zero;
        player.Rb2d.linearVelocity = Vector3.zero;
        _player = player;
    }
    
    internal override void UpdateState(PlayerStateManager playerStateManager, PlayerController player)
    {
        if (player.SprintTimer > 0 && player.IsRunning)
        {
            player.SprintTimer -= Time.deltaTime;
        }

        if (player.SprintTimer <= 0)
        {
            player.IsRunning = false;
        }
        
        
        if (player.IsRunning) return;
        playerStateManager.CheckForTransition( PlayerStateManager.PlayerStateTypes.Attack | PlayerStateManager.PlayerStateTypes.Walking | PlayerStateManager.PlayerStateTypes.Neutral);
        
    }

   

 

    internal override void ExitState(PlayerStateManager playerStateManager, PlayerController player)
    {

//        Debug.Log(player.rb.linearVelocity);
    }
}
