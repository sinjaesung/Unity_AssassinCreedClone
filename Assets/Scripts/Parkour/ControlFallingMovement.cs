using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlFallingMovement : StateMachineBehaviour
{
    private void OnStateEnter(Animator animator,AnimatorStateInfo animatorStateInfo,int layerIndex)
    {
        Debug.Log("ControlFallingMovement Landing OnStateEnter");
        animator.GetComponent<PlayerScript>().HasPlayerControl = false;
    }

    private void OnStateExit(Animator animator,AnimatorStateInfo animatorStateInfo,int layerIndex)
    {
        Debug.Log("ControllFallingMovement Landing OnStateExit");
        animator.GetComponent<PlayerScript>().HasPlayerControl = true;    
    }
}
