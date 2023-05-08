using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LivesController : MonoBehaviour
{
    void Awake()
    {
        if (SceneManager.GetActiveScene().name == "Level2 - Autism")
        {
            for (int i = 0; i < this.transform.childCount; i++)
            {
                GameObject lifeChild = this.transform.GetChild(i).gameObject;
                lifeChild.GetComponent<Image>().color = new Color(0.73f, 0.73f, 0.73f);
            }
        }
    }

}
