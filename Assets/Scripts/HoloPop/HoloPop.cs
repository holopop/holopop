using UnityEngine;
using System.Collections;

// Need a way to place the kicks
// Assign the audio to the kick
// Change the audio to the kick

public class HoloPop : MonoBehaviour
{

    #region Debug
    [Header("Debug Commands")]
    [SerializeField]
    bool debugTap = false;
    #endregion

    void Start()
    {
        Messenger<GameObject>.AddListener(ManagerTunedEvent.ON_TAP, OnTapMessage);
    }

    void OnTapMessage(GameObject targetObject)
    {
        if (targetObject == this.gameObject)
        {
            GameState mode = Managers.GameState.state;

            Debug.Log("I was tapped on " + mode + " mode!");
        }
    }



    void ProcessDebugCommands()
    {
        if (debugTap)
        {
            OnTapMessage(this.gameObject);
            debugTap = false;
        }
    }

    void Update()
    {
        ProcessDebugCommands();
    }
}

