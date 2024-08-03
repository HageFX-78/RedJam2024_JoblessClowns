using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

[Serializable]
public enum SCENE_INDEX
{
    Quit = -1,
    MainMenu,
    GameScene,
}


public static class LevelManager
{
    public static void LoadLevel(SCENE_INDEX sceneIndex)
    {
        switch(sceneIndex)
        {
            case SCENE_INDEX.Quit:
                Application.Quit();
                Debug.Log("Quit");
                break;

            default:
                SceneManager.LoadScene((int)sceneIndex);
                break;
        }

    }
}
