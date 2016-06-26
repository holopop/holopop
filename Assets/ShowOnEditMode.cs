using UnityEngine;
using System.Collections;

public class ShowOnEditMode : MonoBehaviour {
    private MeshRenderer renderer;

	// Use this for initialization
	void Start () {
        this.renderer = GetComponent<MeshRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Managers.GameState.state == GameState.Edit)
        {
            gameObject.layer = 0;
        }
        else
        {
            gameObject.layer = 8;
        }
    }
}
