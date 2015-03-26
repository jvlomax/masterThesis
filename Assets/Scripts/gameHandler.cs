using UnityEngine;
using System.Collections;

public class gameHandler : MonoBehaviour
{
    private int _playerAttack;
    public player player;
    public enemy AI;
    public GameObject flash;
    private bool flashing = false;
    public int PlayerAttack {
        get { return _playerAttack; }
        set { _playerAttack = value; }
    }
    
   

	// Use this for initialization
	void Start ()
	{
	    //player = GetComponent<player>();
	    //AI = GetComponent<enemy>();
	}
	
	// Update is called once per frame
	void Update () {
	    if (flashing){
            flash.GetComponent<GUITexture>().color = new Color(flash.GetComponent<GUITexture>().color.r, flash.GetComponent<GUITexture>().color.g, flash.GetComponent<GUITexture>().color.b, flash.GetComponent<GUITexture>().color.a -0.01f);
            Debug.Log("alpha: " + flash.GetComponent<GUITexture>().color.a); 
	    }
	    if (flash.GetComponent<GUITexture>().color.a == 0){
	        flashing = false;
	    }
	}

    public void endTurn(){
        int attack2 = Random.Range(0, 2);
        Debug.Log("End turn\nAttack1: " + _playerAttack + "\nAttack2: " + attack2);
        //stalemate
        if (_playerAttack == attack2) {
            //Screen flash
            // nothing happens
        }


        //player wins
        if((_playerAttack == 2 && attack2 == 1) ||
            (_playerAttack == 1 && attack2 == 0) ||
            (_playerAttack == 0 && attack2 == 2)) {
            Debug.Log("AI takes damage");
            AI.Damage(-1);
            flash.GetComponent<GUITexture>().color = new Color(flash.GetComponent<GUITexture>().color.r, flash.GetComponent<GUITexture>().color.g, flash.GetComponent<GUITexture>().color.b, 1);
                flashing = true;
                //StartCoroutine(FadeOut(255, 0, 5, flash));
            } else {
            //AI wins
            player.Damage(-1);
            Debug.Log("Player takes Damage");
           // FlashWhenHit();
            //flash.guiTexture.color = new Color(flash.guiTexture.color.r, flash.guiTexture.color.g, flash.guiTexture.color.b, 255);
            
        }
    }

    private IEnumerator FadeOut (int start, int end, float length, GameObject currentObject) { //define Fade parmeters 
      
            Debug.Log("FadeOut");
            for (int i = start; i != end; i--) { //for the length of time 
                currentObject.GetComponent<GUITexture>().color = new Color(i, currentObject.GetComponent<GUITexture>().color.g, currentObject.GetComponent<GUITexture>().color.b, i); //lerp the value of the transparency from the start value to the end value in equal increments 
                Debug.Log("alpha: " + currentObject.GetComponent<GUITexture>().color.a); 
                yield return new WaitForSeconds(1);
                
            } //end for
            currentObject.GetComponent<GUITexture>().color = new Color(currentObject.GetComponent<GUITexture>().color.r, currentObject.GetComponent<GUITexture>().color.g, currentObject.GetComponent<GUITexture>().color.b, end);


    } //end Fade
 
        

  
}
