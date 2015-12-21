using UnityEngine;
using System.Collections;

public class Jump : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.transform.Find("Audio").GetComponent<Sounds>().PlayJumpSound();
    }
}
