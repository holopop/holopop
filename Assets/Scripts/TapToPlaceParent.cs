using UnityEngine;

public class TapToPlaceParent : MonoBehaviour
{
    bool placing = false;

    private Renderer[] renderers;
     
    void Start()
    {
        renderers = this.GetComponentsInChildren<Renderer>();
    }

    void SetTint(Color color)
    {
        foreach (Renderer renderer in renderers)
        {
            renderer.material.color = color;
        }
    }

    // Called by GazeGestureManager when the user performs a Tap gesture
    void OnEditTap()
    {
        // On each Select gesture, toggle whether the user is in placing mode.
        placing = !placing;

        // If the user is in placing mode, display the spatial mapping mesh.
        if (placing)
        {
            Debug.Log("Started moving object");
            Managers.Mapper.DrawVisualMeshes = true;
            SetTint(Color.red);
        }
        // If the user is not in placing mode, hide the spatial mapping mesh.
        else
        {
            Debug.Log("Finished moving object");
            Managers.Mapper.DrawVisualMeshes = false;
            SetTint(new Color(0,0,0,0));
        }
    }

    // Update is called once per frame
    void Update()
    {
        // If the user is in placing mode,
        // update the placement to match the user's gaze.

        if (placing)
        {
            if (Managers.Gaze.hitSpatialMap)
            {
                // Move this object's parent object to
                // where the raycast hit the Spatial Mapping mesh.
                this.transform.parent.position = Managers.Gaze.hitSpatialMapInfo.point;

                // Rotate this object's parent object to face the user.
                Quaternion toQuat = Camera.main.transform.localRotation;
                toQuat.x = 0;
                toQuat.z = 0;
                this.transform.parent.rotation = toQuat;
            }
        }
    }
}