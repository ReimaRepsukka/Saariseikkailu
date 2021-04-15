using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoorStateControl : StateMachineBehaviour
{
    public AudioClip doorSound;

    static bool doorOpen;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.GetComponent<AudioSource>().PlayOneShot(doorSound);

        doorOpen = !doorOpen;


        //Haetaan valon transform komponentti
        Transform l = animator.transform.parent.Find("Valo");

        //Vaihdetaan valon väri
        if(doorOpen)
        {
            l.GetComponent<Light>().color = Color.green;
        }
        else
        {
            l.GetComponent<Light>().color = Color.red;
        }

        
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
