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

    public void SlowStopTime(float duration)
    {
        StartCoroutine(CO_SlowStopTime(duration));
    }
    public IEnumerator CO_SlowStopTime(float duration)
    {
        float currentTime = 0;
        float slowDownFactor = 0.1f;
        while (currentTime < duration)
        {
            currentTime += Time.unscaledDeltaTime;
            Time.timeScale = Mathf.Lerp(1, 0, currentTime / duration);
            yield return null;
        }
        Time.timeScale = 0;
    }
}
