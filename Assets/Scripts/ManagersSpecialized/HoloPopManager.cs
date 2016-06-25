using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class HoloPopConfig
{
    public GameObject prefab;
    public string name;
    public bool debug_drop;
}

public class HoloPopManager : MonoBehaviour, IGameManager {

    public HoloPopConfig[] PopConfigs = new HoloPopConfig[0];
    
    List<HoloPop> HoloPopList;
    public ManagerStatus status { get; private set; }
    Dictionary<string, HoloPopConfig> ConfigByName = new Dictionary<string, HoloPopConfig>();

    #region Debug
    [Header("Debug Commands")]
    [SerializeField] bool destroyLastHoloPop = false;
    [SerializeField] bool destroyAllPops = false;
    [SerializeField] bool playAllPops = false;
    [SerializeField] bool stopAllPops = false;


    #endregion

    public void Startup(NetworkService service)
    {
        HoloPopList = new List<HoloPop>();
        // Build dictionary of configs to name
        foreach (HoloPopConfig config in PopConfigs)
        {

            ConfigByName[config.name] = config;
        }

        status = ManagerStatus.Started;
    }

    private void AddHoloPopToList(HoloPop newHoloPop)
    {        
        HoloPopList.Add(newHoloPop);
    }

    private void destroyLastHoloPopFromList(HoloPop HoloPop)
    {
        HoloPopList.Remove(HoloPop);
    }

    // choose the gazed object, then the gazed scene, then some stupid location
    Vector3 ChooseSpawnPosition()
    {
        if (Managers.Gaze.hitAny)
        {
            return Managers.Gaze.hitAnyInfo.point;
        }
        if (Managers.Gaze.hitSpatialMap)
        {
            return Managers.Gaze.hitSpatialMapInfo.point;
        }
        return new Vector3(0, 0, 2); ;    // choose 2 on z so its in front of the camera
    }

    public void PlayAllPops()
    {
        foreach (HoloPop pop in HoloPopList)
        {
            if (pop.gameObject.GetComponent<AudioSource>())
            {
                pop.gameObject.GetComponent<AudioSource>().Play();
            }
        }
    }

    public void StopAllPops()
    {
        foreach (HoloPop pop in HoloPopList)
        {
            if (pop.gameObject.GetComponent<AudioSource>())
            {
                pop.gameObject.GetComponent<AudioSource>().Stop();
            }
        }
    }

    public void SpawnNewHoloPop(string config_name)
    {
        HoloPopConfig config = ConfigByName[config_name];

        HoloPop b = Instantiate(config.prefab).GetComponent<HoloPop>();
        b.transform.position = ChooseSpawnPosition();
        AddHoloPopToList(b);
    }

    // destroy the last HoloPop in the list
    public bool DestroyLastHoloPop()
    {
        if (HoloPopList.Count > 0)
        {
            HoloPop b = HoloPopList[HoloPopList.Count - 1];
            destroyLastHoloPopFromList(b);
            DestroyObject(b.gameObject);
            // reset the gaze just in case
            Managers.Gaze.ClearHits();
            return true;
        }
        return false;
    }

    public void DestroyAllPops()
    {
        while (DestroyLastHoloPop())
        {
            // loop until none left to destroy
        }
    }

    void ProcessDebugCommands()
    {  
        foreach (HoloPopConfig config in PopConfigs)
        {
            if (config.debug_drop == true)
            {
                SpawnNewHoloPop(config.name);
                config.debug_drop = false;
            }
        }

        if (destroyLastHoloPop)
        {
            DestroyLastHoloPop();
            destroyLastHoloPop = false;
        }

        if (destroyAllPops)
        {
            DestroyAllPops();
            destroyAllPops = false;
        }

        if (playAllPops)
        {
            PlayAllPops();
            playAllPops = false;
        }

        if (stopAllPops)
        {
            StopAllPops();
            stopAllPops = false;
        }

}

void Update()
    {
        ProcessDebugCommands();
    }
     
}
