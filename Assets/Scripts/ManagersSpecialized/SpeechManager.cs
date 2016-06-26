// SpeechManager
// Responsibilities:
//  * Construct and start the KeywordRecognizer
//  * Construct a map from keyword to Action
//  * Invoke the actions when a keyword is recognized

using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Windows.Speech;

public class SpeechManager : MonoBehaviour, IGameManager
{
    public ManagerStatus status { get; private set; }
    KeywordRecognizer keywordRecognizer = null;
    Dictionary<string, System.Action> keywords = new Dictionary<string, System.Action>();

    void BuildGameSpecificKeywordMap()
    {
        /// for each popconfig, make an "Add " recognizer
        foreach (HoloPopConfig config in Managers.HoloPops.PopConfigs)
        {
            string keyword_name = "Add " + config.name;
            keywords.Add(keyword_name, () =>
            {
                Managers.HoloPops.SpawnNewHoloPop(config.name);
            });
        }

        foreach (HoloPopConfig config in Managers.HoloPops.PopConfigs)
        {
            string keyword_name = "Add ten " + config.name;
            keywords.Add(keyword_name, () =>
            {
                for (int i = 0; i < 10; i++)
                {
                    Managers.HoloPops.SpawnNewHoloPop(config.name);
                }
            });
        }

        keywords.Add("Destroy Pop", () =>
        {
            Managers.HoloPops.DestroyLastHoloPop();
        });

        keywords.Add("Destroy All Pops", () =>
        {
            Managers.HoloPops.DestroyAllPops();
        });

        keywords.Add("Edit Mode", () =>
        {
            Managers.GameState.EditMode();
        });

        keywords.Add("Play Mode", () =>
        {
            Managers.GameState.PlayMode();
        });

        keywords.Add("Play All Pops", () =>
        {
            Managers.HoloPops.PlayAllPops();
        });
        keywords.Add("Stop All Pops", () =>
        {
            Managers.HoloPops.StopAllPops();
        });


    }

    public void Startup(NetworkService service)
    {
        // Build the Keywords for this App
        BuildGameSpecificKeywordMap();

        // Tell the KeywordRecognizer about our keywords.
        keywordRecognizer = new KeywordRecognizer(keywords.Keys.ToArray());

        // Register a callback for the KeywordRecognizer and start recognizing!
        keywordRecognizer.OnPhraseRecognized += KeywordRecognizer_OnPhraseRecognized;
        keywordRecognizer.Start();
        status = ManagerStatus.Started;
    }

    private void KeywordRecognizer_OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        System.Action keywordAction;
        if (keywords.TryGetValue(args.text, out keywordAction))
        {
            keywordAction.Invoke();
        }
    }
}