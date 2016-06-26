using UnityEngine;
using System.Collections;

public class TapToPlayParticles : MonoBehaviour {
    public ParticleSystem particles;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnPlayTap()
    {
        particles.Play();
    }
}
