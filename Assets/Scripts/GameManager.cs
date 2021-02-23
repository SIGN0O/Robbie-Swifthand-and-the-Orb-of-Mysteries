using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    private SceneFader fader;
    List<Orb> orbs;
    DeathPose[] deathPoses;
    Door lockedDoor;
    int sceneNum;

    float gameTime;
    bool gameIsOver;

    public int deathNumber;

    private void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        orbs = new List<Orb>();
        sceneNum = SceneManager.sceneCountInBuildSettings;
        DontDestroyOnLoad(this );

    }

    private void Update()
    {
        if (gameIsOver)
            return;
        if (SceneManager.GetActiveScene().buildIndex != 0)
        {
            gameTime += Time.deltaTime;
            UIManager.UpdateTimeUI(gameTime);
        }
    }

    public static void RegisterSceneFader(SceneFader sceneFader )
    {
        instance.fader = sceneFader;

    }

    public static void RegisterDoor(Door door)
    {
        instance.lockedDoor = door;
    }

    public static void RegisterOrb(Orb orb)
    {
        if (instance == null)
            return;
        if (!instance.orbs.Contains(orb))
            instance.orbs.Add(orb);
        if (SceneManager.GetActiveScene().buildIndex != 0)
            UIManager.UpdateOrbUI(instance.orbs.Count);
    }

    public static void PlayerGrabbedOrb(Orb orb)
    {
        if (!instance.orbs.Contains(orb))
            return;
        instance.orbs.Remove(orb);
        if (instance.orbs.Count == 0)
            instance.lockedDoor.Open();
        UIManager.UpdateOrbUI(instance.orbs.Count);
    }

    public static void PlayerWon()
    {
        instance.gameIsOver = true;
        UIManager.DisplayGameOver();
        AudioManager.PlayerWonAudio();
    }

    public static bool GameOver()
    {
        return instance.gameIsOver;
    }
    public static void PlayerDied()
    {
        instance.fader.FadeOut();
        instance.deathNumber++;
        UIManager.UpdateDeathUI(instance.deathNumber);
        instance.Invoke("RestartScene", 1.5f);
    }

    public static void SetGameReset()
    {
        instance.gameIsOver = false;
    }

    public static void NextLevel()
    {
        instance.Invoke("NextScene",0f);
    }
    public static void RestartLevel()
    {
        instance.Invoke("RestartScene",0f);
    }
    /// <summary>
    /// 重新加载当前场景
    /// </summary>
    public void RestartScene()
    {
        instance.orbs.Clear();
        instance.gameIsOver = false;
        UIManager.ResetDisplayGameOver();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    }
    /// <summary>
    /// 进入下一个场景
    /// </summary>
    public void NextScene()
    {
        instance.orbs.Clear();
        if (SceneManager.GetActiveScene().buildIndex + 1 < instance.sceneNum)
        {
            if(SceneManager.GetActiveScene().buildIndex != 0)
            instance.fader.FadeOut();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            deathPoses = FindObjectsOfType<DeathPose>();
            for(int i = 0; i < deathPoses.Length; i++)
            {
                Destroy(deathPoses[i].gameObject);
            }
        }
        else
            PlayerWon();
    }
   
}
