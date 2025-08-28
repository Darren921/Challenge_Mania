using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.InputSystem;
using static PlayerStateManager;

public class PlayerNeutralState : PlayerBaseState
{
   

    internal override void EnterState(PlayerStateManager playerStateManager, PlayerController player )
    {
        player.Rb2d.linearVelocity = Vector2.zero;
        player.Rb2d.angularVelocity = 0;
        Debug.Log("Entered PlayerNeutralState");
    }

    internal override void UpdateState(PlayerStateManager playerStateManager,PlayerController player)
    {
        playerStateManager.CheckForTransition(PlayerStateTypes.Walking | PlayerStateTypes.Running);
    }
     
 
    internal override void FixedUpdateState(PlayerStateManager playerStateManager,PlayerController player)
    {
    }

    internal override void ExitState(PlayerStateManager playerStateManager,PlayerController player)
    {
        Debug.Log("Exit PlayerNeutralState");
    }
}
