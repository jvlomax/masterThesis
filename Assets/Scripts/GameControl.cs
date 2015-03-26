using UnityEngine;
using System.Collections;

public class GameControl : MonoBehaviour
{
    public static GameControl control;
    public float health;
    public float experience;
	// Use this for initialization

    void Awake()
    {
        if (control == null) {

            DontDestroyOnLoad(gameObject);
            control = this;
        } else if (control != this){
            Destroy(gameObject);
        }
        
    }
    
    void Start ()
	{
	    
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnGUI() {
        //GUI.Label(new Rect(10,10,100,30), "Health: " + health );
        //GUI.Label(new Rect(10, 40, 100, 30), "Experience: " + experience);
    }

   
}
