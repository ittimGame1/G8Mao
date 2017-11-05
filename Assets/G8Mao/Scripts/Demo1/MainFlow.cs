using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using DG.Tweening;

public class MainFlow
{
    public static void EnterTestingGame()
    {
        SceneManager.LoadScene("KevinWorkspace");

        //reset GameUI
        GameUI.instance.RunScaleAni(true);
        GameUI.instance.energyBar.ChargeToMax();
        GameUI.instance.angryBar.ChargeToMin();
    }

    public static void LeaveTestingGame()
    {
        if (GameUI.instance != null)
        {
            GameObject.Destroy(GameUI.instance.gameObject);
        }
        EnterStartMenu();
    }

    public static void EnterStartMenu()
    {
        SceneManager.LoadScene("StartMenu");
    }

    public static void EnterStory()
    {
        SceneManager.LoadScene("Story1");
    }

    public static void LeaveStory()
    {
        EnterTestingGame();
    }
}
