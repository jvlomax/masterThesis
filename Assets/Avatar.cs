using UnityEngine;
using System.Collections;

public class Avatar : MonoBehaviour {
    public AudioClip taunt;
    private AudioSource audio;
	// Use this for initialization
	void Start () {
        audio = GetComponent<AudioSource>();
        //gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Appear() {
        audio = GetComponent<AudioSource>();
        gameObject.SetActive(true);
        audio.clip = taunt;
        audio.Play();
    }

    public void Disapear()
    {
        gameObject.SetActive(false);
    }

    void OnMouseDown() {
        Debug.Log("Clicked");
        audio.clip = taunt;
        audio.Play();
    }
}
