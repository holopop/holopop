using UnityEngine;
using System.Collections;

public class PlayAudoOnTap : MonoBehaviour {

    private AudioSource audio;
    private ParticleSystem particles;

	// Use this for initialization
	void Start () {
        this.audio =this.GetComponent<AudioSource>();
        this.particles = this.transform.parent.GetComponentInChildren<ParticleSystem>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnPlayTap()
    {
        if (!this.audio.mute)
        {
            StopAudio();
        }
        else
        {
            PlayAudio();
        }
    }

    public void StopAudio()
    {
        this.audio.mute = true;
        if (this.particles != null)
        {
            this.particles.Stop();
        }
    }

    public void PlayAudio()
    {
        this.audio.mute = false;
        if (this.particles != null)
        {
            this.particles.loop = true;
            this.particles.Play();
        }
    }
}
