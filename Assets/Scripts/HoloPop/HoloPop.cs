using UnityEngine;
using System.Collections;

// Need a way to place the kicks
// Assign the audio to the kick
// Change the audio to the kick

public class HoloPop : MonoBehaviour
{
    // Update is called once per frame
    void Update () {
	    
    }

    void Start()
    {
        Messenger<GameObject>.AddListener(ManagerTunedEvent.ON_TAP, OnTapMessage);

    }

    void OnTapMessage(GameObject targetObject)
    {
        if (targetObject == this.gameObject)
        {
            Debug.Log("I was tapped!");
        }
    }

}

