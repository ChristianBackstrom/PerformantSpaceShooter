using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Scenes;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class RestartGame : MonoBehaviour
{
    private SubScene subScene;
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
