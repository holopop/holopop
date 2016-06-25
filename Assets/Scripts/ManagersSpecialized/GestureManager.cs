// GestureManager.cs
//  Responsibilities:
//      * Create and bind to the GestureRecognizer 
//      * Make sure the gestureRecognizer restarts gesture recognition when the currently focussed object changes
//      * Call OnTap
//  Collaborators and Dependencies:
//      * Binds to the WSA.GestureRecognizer
//      * Relies on the GazeManager to determine what the current object is
//  GameSpecific or Not:
//      * Assumes ontap is relevant
//  Events generated
//      ON_TAP using 
//  Todo sean:
//      * tuning spatialMapMaxDistance

using UnityEngine;
using UnityEngine.VR.WSA.Input;

public class GestureManager :  MonoBehaviour, IGameManager
{
    public ManagerStatus status { get; private set; }

    GameObject gameObjectAtStartOfRecognizer;
    GestureRecognizer recognizer;

    // Use this for initialization
    public void Startup(NetworkService service)
    {
        // Set up a GestureRecognizer to detect Tap gestures.
        recognizer = new GestureRecognizer();
        recognizer.TappedEvent += (source, tapCount, ray) =>
        {
            // Generate an OnTap event
            Messenger<GameObject>.Broadcast(ManagerTunedEvent.ON_TAP, gameObjectAtStartOfRecognizer);
        };
        status = ManagerStatus.Started;
    }

    // Update is called once per frame
    void Update()
    {
        // Figure out which hologram is focused this frame.
        GameObject currentObject = null;
        if (Managers.Gaze.hitAny)
        {
            currentObject = Managers.Gaze.hitAnyInfo.collider.gameObject;
        }
        
        // If the focused object changed this frame,
        // start detecting fresh gestures again.
        if (currentObject != gameObjectAtStartOfRecognizer)
        {
            recognizer.CancelGestures();
            recognizer.StartCapturingGestures();
            gameObjectAtStartOfRecognizer = currentObject;
        }
    }
}