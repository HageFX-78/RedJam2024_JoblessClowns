using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] int normalPropBaseValue = 10;
    [SerializeField] int triggerPropBaseValue = 50;

    [Header("in-game Gui")]
    [SerializeField] private TextMeshProUGUI moneyText;
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private Animator moneyTextAnimator;

    [SerializeField] private float timeLimit = 240f;

    [SerializeField] public List<Prop> propList;


    private float currentTime = 0f;
    public bool gameStarted = false;
    public int moneyAmount = 0; // The amount of money the player has, score but we can use it for something maybe
    void Start()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        StartGame();

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
                return;
            }
            //update time from seconds to minutes and seconds
            int minutes = Mathf.FloorToInt(currentTime / 60.0f);
            int seconds = Mathf.FloorToInt(currentTime % 60f);
            Debug.Log(currentTime);
            timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }


    public void StartGame()
    {
        
        currentTime = timeLimit;
        moneyAmount = 0;
        moneyText.text = "$ " + moneyAmount.ToString();
        gameStarted = true;
    }

    public void AddMoney(int amt)
    {
        moneyAmount += amt;
        moneyText.text = "$ " + moneyAmount.ToString();
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
}
