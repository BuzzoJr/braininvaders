using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level1Manager : MonoBehaviour
{
    public DialogController dialogController;
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
        }
    }

    private void PreLevelDialog()
    {
        dialogController.lines = new string[5];
        dialogController.lines[0] = "You control the green spaceship at the bottom.";
        dialogController.lines[1] = "Your objective is to shoot and kill all the aliens on the screen before they kill you.";
        dialogController.lines[2] = "They will slowly advance towards you and fire missiles at your spaceship.";
        dialogController.lines[3] = "You have 3 chances.";
        dialogController.lines[4] = "Good Luck.";

        GameManager.Instance.UpdateGameState(GameManager.GameState.Dialog);
    }

    private void VictoryDialog()
    {
        dialogController.lines = new string[5];
        dialogController.lines[0] = "Congratulations, human!";
        dialogController.lines[1] = "You have successfully completed the first level of our tests.";
        dialogController.lines[2] = "But don't get too comfortable just yet.";
        dialogController.lines[3] = "The next level will present new challenges and test your skills even further.";
        dialogController.lines[4] = "Prepare yourself, as the aliens will come at you relentlessly, inducing anxiety.";

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
