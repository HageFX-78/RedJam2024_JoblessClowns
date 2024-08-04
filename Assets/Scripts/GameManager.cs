using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using Cinemachine;


public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] int normalPropBaseValue = 10;
    [SerializeField] int triggerPropBaseValue = 50;

    [SerializeField] private GameObject spawnPointParent;
    [SerializeField] private GameObject propBasePrefab;
    [SerializeField] private Material physicsMat;
    [Header("in-game Gui")]
    [SerializeField] private TextMeshProUGUI moneyText;
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private Animator moneyTextAnimator;
    [SerializeField] private GameObject gameOverPanel;

    [SerializeField] private TextMeshProUGUI gameOverMoneyText;
    [SerializeField] private Animator gameOverMoneyTextAnimator;
    [SerializeField] private float timeLimit = 240f;

    [SerializeField] private Animator tutorialPanelAnimator;

    [SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;
    [SerializeField] private CinemachineBasicMultiChannelPerlin _cmCP;
    [SerializeField] private float shakeIntensity = 1f;
    [SerializeField] private float shakeTime = 0.2f;
    private float shakeTimer = 0f;
    [SerializeField] public List<Prop> propList; // Trigger prop list NOT NORMAL


    private float currentTime = 0f;
    public bool gameStarted = false;
    public int moneyAmount = 0; // The amount of money the player has, score but we can use it for something maybe

    private List<KeyValuePair<Transform, bool>> spawnPoints = new List<KeyValuePair<Transform, bool>>();
    
    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        foreach (Transform g in spawnPointParent.GetComponentsInChildren<Transform>())
        {
            spawnPoints.Add(new KeyValuePair<Transform, bool>(g, true));
            Prop prop = propList[Random.Range(0, propList.Count)];

            GameObject propObj = Instantiate(propBasePrefab, g.position, Quaternion.identity);
            //set propObj parent to a child object named clones
            propObj.transform.SetParent(transform.Find("Clones"));
            propObj.GetComponent<PropBehaviour>().InitializeProp(prop);
        }
    }
    void Start()
    {
        //StartGame();
    }
    void Update()
    {
        if(gameStarted)
        {
            currentTime -= Time.deltaTime;
            if(currentTime <= 0)
            {
                currentTime = 0;
                gameStarted = false;
                //Game Over

                gameOverPanel.SetActive(true);
                //TimeManager.Instance.PauseTime();
                gameOverMoneyText.text = moneyAmount.ToString();
                gameOverMoneyTextAnimator.SetTrigger("Trigger");

                SFXManager.Instance.PlaySoundFXClip(AudioStorage.Instance.getAudioClip("gameover"), gameOverPanel.transform, 1f);

                Invoke("StopTime", 0.3f);

                return;
            }
            //update time from seconds to minutes and seconds
            int minutes = Mathf.FloorToInt(currentTime / 60.0f);
            int seconds = Mathf.FloorToInt(currentTime % 60f);

            timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }

        if(shakeTimer >= 0 )
        {
            shakeTimer -= Time.deltaTime;
            if(shakeTimer <= 0)
            {
                StopShake();
            }
        }
    }


    public void StartGame()
    {
        CloseTutorialPanel();
        currentTime = timeLimit;
        moneyAmount = 0;
        moneyText.text = "$ " + moneyAmount.ToString();
        gameStarted = true;
    }

    public void AddMoney(int amt)
    {
        moneyAmount += amt * 1000;
        moneyText.text = moneyAmount.ToString() + " C";
        moneyTextAnimator.SetTrigger("AddCoin");
    }
    public void AddMoney(PropType propType)
    {
        switch (propType)
        {
            case PropType.Normal:
                AddMoney(normalPropBaseValue);
                break;
            case PropType.Trigger:
                AddMoney(triggerPropBaseValue);
                break;
            default:
                break;
        }
        
    }
    public void RemoveMoney(int amt)
    {
        moneyAmount -= amt;
    }

    public void SaveMoney()
    {
        PlayerPrefs.SetInt("Money", moneyAmount);
    }
    public void LoadMoney()
    {
        moneyAmount = PlayerPrefs.HasKey("Money") ? PlayerPrefs.GetInt("Money") : 0;
    }

    public void ShakeCamera()
    {
        CinemachineBasicMultiChannelPerlin _cmCP = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        _cmCP.m_AmplitudeGain = shakeIntensity;

        shakeTimer = shakeTime;
    }
    public void StopShake()
    {
        CinemachineBasicMultiChannelPerlin _cmCP = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        _cmCP.m_AmplitudeGain = 0;
        shakeTimer = 0;
    }
    public void StopTime()
    {
        TimeManager.Instance.SlowStopTime(0.5f);
    }

    public void CloseTutorialPanel()
    {
        tutorialPanelAnimator.SetTrigger("AnimateOut");
    }
}
