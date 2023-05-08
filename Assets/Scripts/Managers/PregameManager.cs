using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PregameManager : MonoBehaviour
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

    private void GameManagerOnGameStateChange(GameManager.GameState state)
    {
        if (state.ToString() == "Playing")
        {
            SceneManager.LoadScene(2);
        }
    }

    void Start()
    {
        PreLevelDialog();
        //Invoke("PreLevelDialog", 1);
    }

    private void PreLevelDialog()
    {
        dialogController.lines = new string[7];
        dialogController.lines[0] = "Hello human.";
        dialogController.lines[1] = "You must be thinking what happened to you.";
        dialogController.lines[2] = "Yes you were abducted.";
        dialogController.lines[3] = "But don't worry.";
        dialogController.lines[4] = "We'll just run some tests and you'll be released.";
        dialogController.lines[5] = "In these tests, we will manipulate your sensations to understand how your species responds to adversities.";
        dialogController.lines[6] = "And for that we'll use something that ou might be familiar with.";
        GameManager.Instance.UpdateGameState(GameManager.GameState.Dialog);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
