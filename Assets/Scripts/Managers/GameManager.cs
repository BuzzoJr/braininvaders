using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameState State;

    public static event System.Action<GameState> OnGameStateChange;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            // Destroy this GameManager object, since it's a duplicate
            Destroy(gameObject);
        }
        else if(Instance == null)
        {
            // Set this GameManager object as the singleton instance
            Instance = this;

            // Mark this GameManager object to persist between scene changes
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start()
    {
        UpdateGameState(GameState.Menu);
    }
    // Start is called before the first frame update
    public void UpdateGameState(GameState newState)
    {
        State = newState;

        switch(newState)
        {
            case GameState.Menu:
                break;
            case GameState.Dialog:
                break;
            case GameState.Playing: 
                break;
            case GameState.Pausing: 
                break;
            case GameState.Victory:
                break;
            case GameState.GameOver:
                break;
        }

        OnGameStateChange?.Invoke(newState);
    }

    public enum GameState
    {
        Menu,
        Dialog,
        Playing,
        Pausing,
        Victory,
        GameOver
    }
}
