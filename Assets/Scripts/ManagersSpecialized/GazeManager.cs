// GazeManager.cs
//  Responsibilities:
//      * Know if and how the current gaze hits any object
//      * Know if and how the most recent gaze(camera transform) hits the spatial map 
//  Collaborators and Dependencies:
//      * Uses UnityEngine.Physics Raycast system to cast rays/find objects
//      * (Implicitly) Relies on SpatialMapper to maintain the spatial map
//  GameSpecific or Not:
//      * Assumes that applications are going to want to know what object or space the gaze is at
//  Todo sean:
//      * tuning spatialMapMaxDistance

using UnityEngine;

public class GazeManager : MonoBehaviour, IGameManager {
    public ManagerStatus status { get; private set; }
    public bool hitAny { get; private set; }
    public RaycastHit hitAnyInfo;
    public bool hitSpatialMap { get; private set; }
    public RaycastHit hitSpatialMapInfo;

    float spatialMapMaxDistance = 30.0f; // sean: this should be from spatial mapper.  also it should be max distance in bounding
                                         // volume ie sqrt(300)=17.3.  However 30.0 is the known good value so leave it till
                                         // I can test

    Transform myTransform;  // cached reference to my Transform
    public void Startup(NetworkService service)
    {
        myTransform = Camera.main.transform;
        status = ManagerStatus.Started;
    }

    void Update()
    {
        hitAny = Physics.Raycast(myTransform.position, myTransform.forward, out hitAnyInfo);

        // from TapToPlaceParent.cs
        hitSpatialMap = Physics.Raycast(myTransform.position, myTransform.forward,
                            out hitSpatialMapInfo, 30.0f, SpatialMapper.PhysicsRaycastMask);
    } 
}
