using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Level3Manager : MonoBehaviour
{
    public DialogController dialogController;
    public AudioSource backgoundMusic;
    public float waitTime = 0.2f;
    //----------press
    //public TextMeshProUGUI pressText;
    //private string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    //private int index;
    //-------Thoughts
    public TextMeshProUGUI thoughtsText;
    private List<string> thoughtsList = new List<string>();
    private int thoughtIndex = 0;
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
        if (state.ToString() == "Victory")
        {
            VictoryDialog();
        }
        if (state.ToString() == "Playing")
        {
            backgoundMusic.Play();
            //StartCoroutine(ChangeLetter());
            StartCoroutine(ChangeThought());
        }
        else
        {
            backgoundMusic.Stop();
        }
    }

    void Start()
    {
        PreLevelDialog();
    }

    private void PreLevelDialog()
    {
        dialogController.lines = new string[3];
        dialogController.lines[0] = "Now we will test your hability to focus.";
        dialogController.lines[1] = "You will have to controll your impulsivity and manage to do the task.";
        dialogController.lines[2] = "Remember, your objective is still to shoot and kill all the aliens before they kill you.";
        GameManager.Instance.UpdateGameState(GameManager.GameState.Dialog);
    }

    private void VictoryDialog()
    {
        dialogController.lines = new string[1];
        dialogController.lines[0] = "Congratulations, human!";
    }

    //IEnumerator ChangeLetter()
    //{
    //    while (true)
    //    {
    //        float red = Random.Range(0f, 1f);
    //        float green = Random.Range(0f, 1f);
    //        float blue = Random.Range(0f, 1f);
    //        pressText.color = new Color(red, green, blue);

    //        index = Random.Range(0, alphabet.Length);
    //        pressText.text = "Press " + alphabet[index];
    //        // Wait for 2 seconds before changing the color again
    //        yield return new WaitForSeconds(3f);
    //    }
    //}

    IEnumerator ChangeThought()
    {
        thoughtsList.Add("Focus.");
        thoughtsList.Add("I wonder what will happen if I do this?");
        thoughtsList.Add("I need to remember to pay that bill.");
        thoughtsList.Add("I should write this down before I forget.");
        thoughtsList.Add("I need to finish this task");
        thoughtsList.Add("Did I forget something important?");
        thoughtsList.Add("I wonder if I turned off the stove.");
        thoughtsList.Add("I can't focus on this, it's too boring.");
        Debug.Log(thoughtsList.Count);

        while (true)
        {
            float red = Random.Range(0f, 1f);
            float green = Random.Range(0f, 1f);
            float blue = Random.Range(0f, 1f);
            thoughtsText.color = new Color(red, green, blue);

            thoughtsText.text = thoughtsList[thoughtIndex];
            // Wait for 2 seconds before changing the color again
            if (thoughtIndex < thoughtsList.Count - 1)
            {
                thoughtIndex++;
            }
            else
            {
                thoughtIndex = 0;
            }
            yield return new WaitForSeconds(3f);
        }
    }
}
