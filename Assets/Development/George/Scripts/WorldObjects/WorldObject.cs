using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldObject : MonoBehaviour
{

    public string objectName;
    public Texture2D buildImage;
    public int cost, sellValue, hitPoints, maxHitPoints;

    protected Player myPlayer;
    protected string[] actions = { };
    protected bool currentlySelected = false;

    protected virtual void Awake()
    {
        
    }

    // Use this for initialization
    protected virtual void Start ()
    {
        myPlayer = transform.root.GetComponentInChildren<Player>();
	}
	
	// Update is called once per frame
	protected virtual void Update ()
    {
		
	}


    public void SetSelection(bool selected)
    {
        currentlySelected = selected;
    }

    public string[] GetActions()
    {
        return actions;
    }

    public virtual void PerformAction (string actionToPerform)
    {

    }
}
