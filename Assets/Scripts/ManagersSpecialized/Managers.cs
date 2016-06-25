// Managers.cs
//
//  Responsibilities:
//      * Provide a central and consistent explicit interface to all game singletons
//      * Provides an explicit startup sequence
//
//  When should I write a manager versus doing it in code
//      the managers provide a layering, a place for tuning and a sensible level of cohesion
//      managers can couple to other managers to keep the project simple.
//      * All writes into the hololens apis should be in a manager.  This allows a common
//        place to tune the manager.
//      * Application specific tuning should be done by pasta hacks in the managers
//          * the managers exist as a place to do this tuning.    
//      * I call it tunedmanager.  but unity people probably already know it's a script so it's "tuned".  Ha (Hacked)
//
// sean:
// How much should be made as a framework and how much should be copy-pasta
//   * copy-pasta but call out GameSpecific
//   * unity is the IAP of visual studio
//   * unity is the MSWord table formatter of typesetting

// Scopes: Game, Scene, 

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(SpatialMapper))]
[RequireComponent(typeof(GazeManager))]
[RequireComponent(typeof(GestureManager))]
[RequireComponent(typeof(SpeechManager))]

[RequireComponent(typeof(HoloPopManager))]
[RequireComponent(typeof(GameStateManager))]


public class Managers : MonoBehaviour {
    public static GazeManager Gaze { get; private set; }        // 
    public static GestureManager Gesture {get; private set;}
    public static SpeechManager Speech { get; private set; }
    public static SpatialMapper Mapper { get; private set; }
    public static HoloPopManager HoloPops { get; private set; }
    public static GameStateManager GameState { get; private set; }

    private List<IGameManager> _startSequence;
    
    void Awake() {
        DontDestroyOnLoad(gameObject);  // persist managers object between scenes
                                        // why not just make it static

        Mapper = GetComponent<SpatialMapper>();
        Gaze = GetComponent<GazeManager>();
        Gesture = GetComponent<GestureManager>();
        Speech = GetComponent<SpeechManager>();
        HoloPops = GetComponent<HoloPopManager>();
        GameState = GetComponent<GameStateManager>();

        _startSequence = new List<IGameManager>();
        _startSequence.Add(Mapper);
        _startSequence.Add(Gaze);
        _startSequence.Add(Gesture);
        _startSequence.Add(Speech);
        _startSequence.Add(HoloPops);
        _startSequence.Add(GameState);
        StartCoroutine(StartupManagers());
    }

    private IEnumerator StartupManagers() {
        NetworkService network = new NetworkService();

        foreach (IGameManager manager in _startSequence) {
            manager.Startup(network);
        }

        yield return null;

        int numModules = _startSequence.Count;
        int numReady = 0;
        
        while (numReady < numModules) {
            int lastReady = numReady;
            numReady = 0;
            
            foreach (IGameManager manager in _startSequence) {
                if (manager.status == ManagerStatus.Started) {
                    numReady++;
                }
            }

            if (numReady > lastReady)
            {
                Debug.Log("Progress: " + numReady + "/" + numModules);
                Messenger<int, int>.Broadcast(
                    StartupEvent.MANAGERS_PROGRESS, numReady, numModules, MessengerMode.DONT_REQUIRE_LISTENER);
            }
            
            yield return null;
        }
        
        Debug.Log("All managers started up");
        Messenger.Broadcast(StartupEvent.MANAGERS_STARTED, MessengerMode.DONT_REQUIRE_LISTENER);
    }
}
