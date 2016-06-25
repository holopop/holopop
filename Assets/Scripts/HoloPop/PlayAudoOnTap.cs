using UnityEngine;
using System.Collections;

public class PlayAudoOnTap : MonoBehaviour {

    public AudioSource audio;

	// Use this for initialization
	void Start () {
        this.audio = this.GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnPlayTap()
    {
        if (this.audio.isPlaying)
        {
            this.audio.Stop();
        }
        else
        {
            this.audio.Play();
        }
    }
}
