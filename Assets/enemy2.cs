using UnityEngine;
using System.Collections;

public class enemy2 : MonoBehaviour {
    public Animator anim;
    private gameHandler gh;
	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
	    gh = GetComponent<gameHandler>();
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
}
