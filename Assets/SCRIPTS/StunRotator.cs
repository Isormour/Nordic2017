﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunRotator : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        this.transform.Rotate(0, 10.0f, 0);
	}
}
