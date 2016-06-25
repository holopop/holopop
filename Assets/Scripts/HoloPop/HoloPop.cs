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

            // Using SendMessage below since these functions should not expect any parameters.
            // Not sure if we would need to use the Messenger API here.
            switch (mode)
            {
                case GameState.Edit:
                    // The following will work as long as the prefabs have behaviors 
                    // attached that implement the required methods (OnEditTap and OnPlayTap)
                    // We will have a generic behavior for editing, and we might have custom 
                    // behaviors for playing depending on what the game object needs to do.
                    gameObject.SendMessage("OnEditTap");
                    break;
                case GameState.Play:
                    gameObject.SendMessage("OnPlayTap");
                    break;
                default:
                    Debug.Log("Unhandled tap while on unknown state:" + mode);
                    break;
            }
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

