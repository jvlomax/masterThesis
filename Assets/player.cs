using System;
using UnityEngine;
using System.Collections;

public class player : MonoBehaviour {
    public Animator anim;
    public gameHandler gh;
    private int _health;
   
    public int Health
    {
        get { return _health; }
        set { _health = value; }
    }
	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
	   // gh = GetComponent<gameHandler>();
	}
	
	// Update is called once per frame
	void Update () {
        if(Input.GetKeyDown("space")) {
            anim.SetTrigger("enemy2Shot");
        }
	}

    public void attack(int type){
        Debug.Log("attack: " + type);
        gh.PlayerAttack = type;
        gh.endTurn();
    }

    public void Damage(int amount)
    {
        _health += amount;
        Transform healthbar = transform.Find("ui_healthDisplay/HealthBar");
        
        
            healthbar.transform.localScale = new Vector3(healthbar.transform.localScale.x - 0.33f, 1, 1);
       
        
    }
}
