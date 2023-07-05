using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class Level5Manager : MonoBehaviour
{
    public DialogController dialogController;
    public GameObject postProcessing;
    private AudioSource victoryMusic;
    public AudioMixer audioMixer;
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
        victoryMusic = GetComponent<AudioSource>();
        //Invoke("PreLevelDialog", 1);
    }

    private void GameManagerOnGameStateChange(GameManager.GameState state)
    {
        if (state.ToString() == "Victory")
        {
            VictoryDialog();
            postProcessing.SetActive(false);
            audioMixer.SetFloat("GamePitch", 1f);
            victoryMusic.Play();
        }

        if(state.ToString() == "Playing" || state.ToString() == "Dialog")
        {
            audioMixer.SetFloat("GamePitch", 0.6f);
        }
        else
        {
            audioMixer.SetFloat("GamePitch", 1f);
        }
    }

    private void PreLevelDialog()
    {
        dialogController.lines = new string[4];
        dialogController.lines[0] = "You are now facing the most challenging test of all: depression.";
        dialogController.lines[1] = "Depression can make it difficult to find motivation or enjoyment in activities.";
        dialogController.lines[2] = "Despite the overwhelming darkness, your objective remains the same: shoot and eliminate all the aliens before they overpower you.";
        dialogController.lines[3] = "Remember to seek support if needed.";

        GameManager.Instance.UpdateGameState(GameManager.GameState.Dialog);
    }

    private void VictoryDialog()
    {
        dialogController.lines = new string[4];
        dialogController.lines[0] = "CONGRATULATIONS, HUMAN!";
        dialogController.lines[1] = "You have shown incredible strength in facing the depths of depression.";
        dialogController.lines[2] = "We hope this experience has brought you insight and empathy.";
        dialogController.lines[3] = "While this concludes our tests, please remember that there is always help available if you or someone you know is struggling with depression.";

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
