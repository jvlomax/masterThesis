using UnityEngine;
using System.Collections;

public class GuardianScreenHandler : MonoBehaviour {
    public Avatar avatar;
	// Use this for initialization
	void Start () {
	   Application.LoadLevel(1);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

     void OnLevelWasLoaded(int level) {
      
        
        avatar.Disapear();
    // yield return new WaitForSeconds(0.5f);
        avatar.Appear();
         
    }
}
