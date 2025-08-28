using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class PlayerMovingState : PlayerBaseState
{
    protected PlayerController _player; 
    protected Vector2 moveDir;
    protected Vector2 _smoothedMoveDir;
    protected Vector2 _smoothedMoveVelocity;

    protected virtual float moveSpeed => 1;
    
    internal override void EnterState(PlayerStateManager playerStateManager, PlayerController player)
    {
      Debug.Log("Entered " + playerStateManager.currentState);
        _player = player;
    }

    internal override void FixedUpdateState(PlayerStateManager playerStateManager, PlayerController player)
    {
        setMoveDir(player.Movement);
        smoothMovement();
        applyVelocity(player);
    }

    protected void applyVelocity(PlayerController player)
    {
        var velocity = new Vector2(_smoothedMoveDir.x * moveSpeed,_smoothedMoveDir.y * moveSpeed);
      
        player.Rb2d.linearVelocity = velocity;    
    }

    protected void smoothMovement()
    {
        _smoothedMoveDir = Vector2.SmoothDamp(_smoothedMoveDir, moveDir, ref _smoothedMoveVelocity, 0.1f);
    }

    protected void setMoveDir(Vector2 newDir)
    {
        moveDir = newDir.normalized;
    }
 

   
    internal override void ExitState(PlayerStateManager playerStateManager, PlayerController player)
    {
      //   player.rb.linearVelocity = Vector3.zero;
        _smoothedMoveVelocity = Vector2.zero;
        _smoothedMoveDir = Vector2.zero;
        player.Rb2d.linearVelocity = Vector3.zero;

    }
}
