﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAudio : MonoBehaviour
{

	// Use this for initialization
	void Start ()
    {
        AudioSource audio = GetComponent<AudioSource>();
        audio.Play();
	}
	
	
}
