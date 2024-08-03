using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTransitionHandler : MonoBehaviour
{
    public SCENE_INDEX sceneToLoadto;

    public void LoadLevel()
    {
        LevelManager.LoadLevel(sceneToLoadto);
    }
}
