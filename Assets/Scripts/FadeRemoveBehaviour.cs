using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeRemoveBehaviour : StateMachineBehaviour
{
    public float fadeTime = 0.5f;
    private float timeElapsed = 0.0f;
    SpriteRenderer spriteRenderer;
    GameObject objectToRemove;
    Color startColor;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timeElapsed = 0.0f;
        spriteRenderer = animator.GetComponent<SpriteRenderer>();
        objectToRemove = animator.transform.parent.gameObject;
        startColor = spriteRenderer.color;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timeElapsed += Time.deltaTime;
        float alpha = startColor.a * (1.0f - timeElapsed / fadeTime);
        spriteRenderer.color = new Color(1.0f, 1.0f, 1.0f, alpha);
        if (timeElapsed > fadeTime)
        {
            Destroy(objectToRemove);
        }

    }


}
