using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTransitionHandler : MonoBehaviour
{
    public SCENE_INDEX sceneToLoadto;

    [SerializeField] private AudioClip buttonSoundClip;

    public void LoadLevel()
    {
        SFXManager.Instance.PlaySoundFXClip(buttonSoundClip, transform, 1f);

        Invoke(nameof(HandleLoadScene), buttonSoundClip.length);
    }

    private void HandleLoadScene()
    {
        LevelManager.LoadLevel(sceneToLoadto);
    }
}
