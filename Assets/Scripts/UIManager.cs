using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    private static UIManager instance;
    public TextMeshProUGUI orbText, timeText, deathText, gameOverText;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(this);
    }

    public static void UpdateOrbUI(int orbCount)
    {
        instance.orbText.text = orbCount.ToString();
    }
    public static void UpdateDeathUI(int deathNumber)
    {
        instance.deathText.text = deathNumber.ToString();
    }
    public static void UpdateTimeUI(float time)
    {
        int minutes = (int)(time / 60);
        float seconds = time % 60;
        instance.timeText.text = minutes.ToString("00") + ":" + seconds.ToString("00");
    }
    public static void DisplayGameOver()
    {
        instance.gameOverText.enabled = true;
    }
    public static void ResetDisplayGameOver()
    {
        instance.gameOverText.enabled = false;
    }
}
