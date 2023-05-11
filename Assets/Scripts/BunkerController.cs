using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BunkerController : MonoBehaviour
{
    public GameObject spriteMaskPrefab;
    // Start is called before the first frame update
    void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.gameObject.layer == LayerMask.NameToLayer("Invader"))
        {
            gameObject.SetActive(false);
        }
    }

    //void OnTriggerExit2D(Collider2D other)
    //{
        
    //    Vector3 spawnPosition = other.transform.position;
    //    if (other.gameObject.layer == LayerMask.NameToLayer("Missile"))
    //    {
    //        spawnPosition.y -= 0.45f;
    //    }
    //    Instantiate(spriteMaskPrefab, spawnPosition, Quaternion.identity, transform);
    //}

}
