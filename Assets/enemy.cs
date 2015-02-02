using UnityEngine;
using System.Collections;

public class enemy : MonoBehaviour {
    private int _health = 3;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Damage(int amount) {
        _health += amount;
        Transform healthbar = transform.Find("ui_healthDisplay/HealthBar");

   
            healthbar.transform.localScale = new Vector3(healthbar.transform.localScale.x - 0.33f, 1, 1);
       

    }
}
