using Neoxider.Bonus;
using Neoxider.Shop;
using System;
using TMPro;
using UnityEngine;

[System.Serializable]
public class SavesData
{
    public int freeSpin;
    public int totalBet;
    public int totalWin;
    public int totalSpin;
    public int[] animalClick = new int[48];
    public bool[] animalBuy = new bool[48];
    public bool daylyBonusTake = false;
    public int StepTutorial;
    public bool IsTutorialComplated = false;
}

public class Stats : MonoBehaviour
{
    public static Stats Instance => instance;

    private static Stats instance;
    public SetText textFreeSpin;
    public SetText textTotalBet;
    public SetText textTotalWin;
    public TMP_Text textTotalSpin;
    public TimeReward TimeReward;

    public SavesData data;

    public void SetStats(int bet, int win, int spin)
    {
        data.totalBet = bet;
        data.totalWin = win;
        data.totalSpin = spin;
    }

    private void Awake()
    {
        print("stats instance");
        data.animalClick = new int[48];

        Load();
        UpdateText();
        instance = this;
        TimeReward.OnRewardAvailable.AddListener(GetReward);
        Application.targetFrameRate = 60;
    }

    private void GetReward()
    {
        Debug.Log("GetDailyReward");
        for (int i = 0; i < data.animalClick.Length; i++)
            data.animalClick[i] = 50;

        //Money.Instance.Add(50);
        Save();

        TimeReward.TakeReward();
    }

    private void Load()
    {
        string jsonData = PlayerPrefs.GetString(nameof(SavesData), String.Empty);

        if (!String.IsNullOrEmpty(jsonData))
        {
            data = JsonUtility.FromJson<SavesData>(jsonData);
        }
    }

    private void OnApplicationQuit() => Save();
    

    private void Save()
    {
        string jsonData = JsonUtility.ToJson(data);
        PlayerPrefs.SetString(nameof(SavesData), jsonData);
    }

    private void UpdateText()
    {
        textFreeSpin.Set(data.freeSpin);
        textTotalBet.text = data.totalBet.ToString();
        textTotalWin.text = data.totalWin.ToString();
        textTotalSpin.text = data.totalSpin.ToString();
    }

    public void AddTotalBet(int count)
    {
        data.totalBet += count;
        data.totalSpin += 1;
        UpdateText();
        Save();
    }

    public void AddTotalWin(int count)
    {
        data.totalWin += count;
        UpdateText();
        Save();
    }

    public void AddFreeSpin(int count)
    {
        data.freeSpin += count;
        UpdateText();
        Save();
    }

    public void SpendFreeSpin()
    {
        data.freeSpin -=1;
        UpdateText();
        Save();
    }
}