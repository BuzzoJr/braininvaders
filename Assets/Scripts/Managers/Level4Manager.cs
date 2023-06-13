using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level4Manager : MonoBehaviour
{
    public DialogController dialogController;
    public GameObject anxietyClock;
    // Start is called before the first frame update
    void Awake()
    {
        GameManager.OnGameStateChange += GameManagerOnGameStateChange;
    }
    void OnDestroy()
    {
        GameManager.OnGameStateChange -= GameManagerOnGameStateChange;
    }
    void Start()
    {
        PreLevelDialog();
        //Invoke("PreLevelDialog", 1);
    }

    private void GameManagerOnGameStateChange(GameManager.GameState state)
    {
        if (state.ToString() == "Victory")
        {
            VictoryDialog();
        }else if (state.ToString() == "Playing")
        {
            anxietyClock.SetActive(true);
        }
    }

    private void PreLevelDialog()
    {
        dialogController.lines = new string[3];
        dialogController.lines[0] = "The aliens will be more relentless in this level, and you may experience a rise in anxiety as you progress.";
        dialogController.lines[1] = "Stay calm and remember to take deep breaths when needed.";
        dialogController.lines[2] = "Your objective remains the same: shoot and eliminate all the aliens before they overpower you.";

        GameManager.Instance.UpdateGameState(GameManager.GameState.Dialog);
    }

    private void VictoryDialog()
    {
        dialogController.lines = new string[4];
        dialogController.lines[0] = "Congratulations, human!";
        dialogController.lines[1] = "You've conquered this level, demonstrating resilience in the face of anxiety.";
        dialogController.lines[2] = "As you advance, the game will continue to test your ability to manage your anxiety and prevail.";
        dialogController.lines[3] = "We'll be altering your perception to simulate what it might be like for an individual with ADHD.";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
