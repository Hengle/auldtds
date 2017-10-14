using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAnimationPlayer : MonoBehaviour {

    public string idleAnimation;
    public string moveAnimation;
    public string takeHitAnimation;
    public string dieAnimation;
    public string runAnimation;
    public string blockAnimation;

    public string currentAnimation;
    
    
    // Use this for initialization
	void Start ()
    {
        currentAnimation = idleAnimation;
        
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void CrossFadeAnimation(string animationName)
    {
        if (this.gameObject.tag != "RTSUnit")
        {
            currentAnimation = animationName;
            GetComponent<Animation>().CrossFade(animationName);
        }
        
        
    }
}
