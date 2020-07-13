using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{

    public Button green;
    public Button red;
    public Button gold;
    public int level = 1;

    void Start()
    {
        var unlocked = PlayerProgress.levelUnlocked;
        green.GetComponentInChildren<Text>().text = level.ToString();
        gold.GetComponentInChildren<Text>().text = level.ToString();
        red.GetComponentInChildren<Text>().text = "X";

        green.gameObject.SetActive(level < unlocked);
        gold.gameObject.SetActive(level == unlocked);
        red.gameObject.SetActive(level > unlocked);
    }

    // Update is called once per frame
    public void Load()
    {
        if (level <= PlayerProgress.levelUnlocked)
            SceneManager.LoadScene($"Level_{level}");
    }
}

public static class PlayerProgress
{
    public static int levelUnlocked 
    { 
        get { return PlayerPrefs.GetInt("level_unlocked", 1); } 
        set { 
            PlayerPrefs.SetInt("level_unlocked", value);
            PlayerPrefs.Save();
        } 
    }

    public static int totalLevels = 6;
}
