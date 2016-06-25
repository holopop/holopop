using UnityEngine;
using System.Collections;

public class SampleEditBehavior : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    // Handle taps on edit mode

    void OnEditTap()
    {
        Debug.Log("Editing this object!");
        // This is where we will add functionality for moving objects around.
    }
}
