using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverManager : MonoBehaviour
{
    private AudioSource gameoverSound;
    // Start is called before the first frame update
    void Awake()
    {
        GameManager.OnGameStateChange += GameManagerOnGameStateChange;
        gameoverSound = GetComponent<AudioSource>();
    }
    void OnDestroy()
    {
        GameManager.OnGameStateChange -= GameManagerOnGameStateChange;
    }

    private void GameManagerOnGameStateChange(GameManager.GameState state)
    {
        if (state.ToString() == "GameOver")
        {
            transform.GetChild(0).gameObject.SetActive(true);
            gameoverSound.Play();
        }
        else
        {
            transform.GetChild(0).gameObject.SetActive(false);
        }
    }
}
