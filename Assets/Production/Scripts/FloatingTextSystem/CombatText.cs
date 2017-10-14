using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatText : MonoBehaviour
{

    private float speed;
    private Vector3 direction;
    private float fadeTime;

    public AnimationClip criticalAnimationClip;
    private bool critHit;
    
    	
	// Update is called once per frame
	void Update ()
    {
        if (!critHit)
        {
            float translation = speed * Time.deltaTime;
            transform.Translate(direction * translation);
        }
        transform.LookAt(2 * transform.position - CombatTextManager.Instance.playerViewPoint.transform.position);
	}

    public void InitializeText(float speed, Vector3 direction, float fadeTime, bool critical)
    {
        this.speed = speed;
        this.direction = direction;
        this.fadeTime = fadeTime;
        this.critHit = critical;

        if (critHit)
        {
            GetComponent<Animator>().SetTrigger("Critical");
            StartCoroutine(Critical());
        }
        else
        {
            StartCoroutine(FadeOutText());
        }
       
    }

    private IEnumerator Critical()
    {
        yield return new WaitForSeconds(criticalAnimationClip.length);
        critHit = false;
        StartCoroutine(FadeOutText());
    }

    private IEnumerator FadeOutText()
    {
        float startAlpha = GetComponent<Text>().color.a;
        float rate = 1.0f / fadeTime;
        float progress = 0.0f;

        while (progress <1.0)
        {
            Color tmpColor = GetComponent<Text>().color;
            GetComponent<Text>().color = new Color(tmpColor.r, tmpColor.g, tmpColor.b, Mathf.Lerp(startAlpha, 0, progress));

            progress += rate * Time.deltaTime;

            yield return null;
        }
        Destroy(gameObject);
        

    }
}
