using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 🌟 继承自 NPCBaseFSM
public class Hide : NPCBaseFSM
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex); 
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
            NPC.GetComponent<RobotGuardAI>().CleverHide();       
            NPC.GetComponent<RobotGuardAI>().RegenerateHealth();
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
    }
}