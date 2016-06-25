using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HoloPopManager : MonoBehaviour, IGameManager {
    public GameObject HoloPopPrefab;
    public GameObject MainSceneObject;

    List<HoloPop> HoloPopList;
    public ManagerStatus status { get; private set; }

    #region Debug
    [Header("Debug Commands")]
    [SerializeField] bool makeHoloPop = false;
    [SerializeField] bool removeHoloPop = false;
    #endregion

    public void Startup(NetworkService service)
    {
        HoloPopList = new List<HoloPop>();
        status = ManagerStatus.Started;
    }

    private void AddHoloPop(HoloPop newHoloPop)
    {        
        HoloPopList.Add(newHoloPop);
    }

    private void RemoveHoloPop(HoloPop HoloPop)
    {
        HoloPopList.Remove(HoloPop);
    }

    public void MakeHoloPop()
    {
        HoloPop b = Instantiate(HoloPopPrefab).GetComponent<HoloPop>();
        b.transform.position = new Vector3(0, 0, 2);    // choose 2 on z so its in front of the camera
        b.transform.parent = MainSceneObject.transform;
        AddHoloPop(b);
    }

    // destroy the last HoloPop in the list
    public bool DestroyHoloPop()
    {
        if (HoloPopList.Count > 0)
        {
            HoloPop b = HoloPopList[HoloPopList.Count - 1];
            RemoveHoloPop(b);
            DestroyObject(b.gameObject);
            return true;
        }
        return false;
    }

    void ProcessDebugCommands()
    {
        if (makeHoloPop)
        {
            MakeHoloPop();
            makeHoloPop = false;
        }
        if (removeHoloPop)
        {
            DestroyHoloPop();
            removeHoloPop = false;
        }
    }

    void Update()
    {
        ProcessDebugCommands();
    }
     
}
