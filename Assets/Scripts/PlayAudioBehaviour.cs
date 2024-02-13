using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAudioBehaviour : StateMachineBehaviour
{
    public AudioClip clip;
    public float volume = 1f;

    public bool playOnEnter = true, playOnExit = false, playAfterDelay = false;

    //delayed sound timer
    public float delay = 0.25f;
    private float timeSinceEntered = 0f;
    private bool hasExited = false;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (playOnEnter)
        {
            AudioSource.PlayClipAtPoint(clip, animator.transform.position, volume);
        }
        timeSinceEntered = 0f;
        hasExited = false;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (playAfterDelay && !hasExited)
        {
            timeSinceEntered += Time.deltaTime;
            if (timeSinceEntered >= delay)
            {
                AudioSource.PlayClipAtPoint(clip, animator.transform.position, volume);
                hasExited = true;
            }
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (playOnExit)
        {
            AudioSource.PlayClipAtPoint(clip, animator.transform.position, volume);
        }

    }

}
