﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    public string sceneToLoad;


    private void OnTriggerEnter2D(Collider2D other)
    {
        if ((other.CompareTag("Player")|| other.CompareTag("PlayerPuxle")) && !other.isTrigger)
        {
            SceneManager.LoadScene(sceneToLoad, LoadSceneMode.Single);
        }
    }
}
