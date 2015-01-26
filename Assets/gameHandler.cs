using UnityEngine;
using System.Collections;

public class gameHandler : MonoBehaviour
{
    private int playerAttack;

    public int PlayerAttack {
        get { return playerAttack; }
        set { playerAttack = value; }
    }
    
   

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        
	}

    public void endTurn(){
        int attack2 = Random.RandomRange(0, 2);
        Debug.Log("End turn\nAttack1: " + playerAttack + "\nAttack2: " + attack2);
    }

  
}
