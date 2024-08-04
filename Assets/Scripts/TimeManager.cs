using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance;

    public static bool isTimePaused = false;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }


    public void PauseTime()
    {
        isTimePaused = true;
        Time.timeScale = 0;
    }

    public void ResumeTime()
    {
        isTimePaused = false;
        Time.timeScale = 1;
    }
}
