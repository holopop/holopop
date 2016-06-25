using UnityEngine;
using System.Collections;

public enum GameState { Edit, Play };

public class GameStateManager : MonoBehaviour, IGameManager {
    public GameState state { get; private set; }
    public ManagerStatus status { get; private set; }

    #region Debug
    [Header("Debug Commands")]
    [SerializeField]
    bool playMode = false;
    [SerializeField]
    bool editMode = false;
    [SerializeField]
    string currentMode = "";
    #endregion


    public void Startup(NetworkService service)
    {
        // Start in edit mode
        state = GameState.Edit;

        SetMode(GameState.Edit);
    }

    public void SetMode(GameState mode)
    {
        state = mode;
        this.currentMode = mode.ToString();
    }

    public void PlayMode()
    {
        SetMode(GameState.Play);
    }

    public void EditMode()
    {
        SetMode(GameState.Edit);
    }



    void ProcessDebugCommands()
    {
        if (playMode)
        {
            PlayMode();
            playMode = false;
        }
        if (editMode)
        {
            EditMode();
            editMode = false;
        }
    }

    void Update()
    {
        ProcessDebugCommands();
    }
}
