using UnityEngine;
using System.Collections;

public class Hit : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        var transform = animator.transform;
        transform.Find("Audio").GetComponent<Sounds>().PlayHitSound();
        transform.GetComponent<PlayerController>().FreezeControls();
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.transform.GetComponent<PlayerController>().UnfreezeControls();
    }
}
