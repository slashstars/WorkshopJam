using UnityEngine;
using System.Collections;

public class Dead : StateMachineBehaviour
{

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.transform.Find("Audio").GetComponent<Sounds>().PlayDeadSound();
    }
}
