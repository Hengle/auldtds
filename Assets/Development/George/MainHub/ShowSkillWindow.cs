using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowSkillWindow : MonoBehaviour
{
    public float animationSpeed = -1.0f;
    public bool skillWindowVisible = false;

    private Animator windowAnimator;

	// Use this for initialization
	void Start ()
    {
        windowAnimator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            skillWindowVisible = !skillWindowVisible;
        }

        if (skillWindowVisible)
        {
            animationSpeed = 1.0f;
            windowAnimator.SetFloat("AnimationSpeed", animationSpeed);
        }
        else
        {
            animationSpeed = -1.0f;
            windowAnimator.SetFloat("AnimationSpeed", animationSpeed);
        }
	}
}
