using System;
using UnityEngine;
using UnityEngine.Events;

public class DailyCheck : MonoBehaviour
{
    public UnityEvent onNewDay;

    private const string LastCheckKey = "LastCheckDate";

    private void Start()
    {
        CheckForNewDay();
    }

    private void CheckForNewDay()
    {
        string lastCheck = PlayerPrefs.GetString(LastCheckKey, string.Empty);
        DateTime lastCheckDate;

        if (string.IsNullOrEmpty(lastCheck) || !DateTime.TryParse(lastCheck, out lastCheckDate))
        {
            onNewDay.Invoke();
            SaveCurrentDate();
        }
        else
        {
            DateTime currentDate = DateTime.Now.Date;

            if (currentDate > lastCheckDate)
            {
                onNewDay.Invoke();
                SaveCurrentDate();
            }
        }
    }

    private void SaveCurrentDate()
    {
        PlayerPrefs.SetString(LastCheckKey, DateTime.Now.Date.ToString());
        PlayerPrefs.Save();
    }
}
