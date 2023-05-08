using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class PauseManager : MonoBehaviour
{
    public AudioMixer audioMixer;
    public static bool isPaused = false;
    private string currentState;

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
        currentState = state.ToString();
        if (currentState == "Pausing")
        {
            transform.GetChild(0).gameObject.SetActive(true);
            isPaused = true;
        }
        else
        {
            transform.GetChild(0).gameObject.SetActive(false);
            isPaused = false;
        }
    }
    // Start is called before the first frame update

    public void ResumeButton()
    {
        GameManager.Instance.UpdateGameState(GameManager.GameState.Playing);
    }

    public void QuitButton()
    {
        SceneManager.LoadScene(0);
    }

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("GlobalVolume", volume);
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeButton();
            }
            else if(currentState == "Playing")
            {
                GameManager.Instance.UpdateGameState(GameManager.GameState.Pausing);
            }
        }
    }
}
