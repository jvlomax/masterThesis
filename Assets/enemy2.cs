﻿using UnityEngine;
using System.Collections;

public class enemy2 : MonoBehaviour {
    public Animator anim;
	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        if(Input.GetKeyDown("space")) {
            anim.SetTrigger("enemy2Shot");
        }
	}
}
