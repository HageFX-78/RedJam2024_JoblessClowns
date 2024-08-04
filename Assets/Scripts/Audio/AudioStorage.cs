using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioStorage : MonoBehaviour
{
    public static AudioStorage Instance;

    [Header("Audio Clips")]
    [SerializeField] private List<AudioClip> audioClips;
    
    
    void Start()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public AudioClip getAudioClip(string name)
    {
        return audioClips.Find(clip => clip.name == name);
    }
}
