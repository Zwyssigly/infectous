using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayButtons : MonoBehaviour
{
    private int level;

    public Button nextButton;
    public Text winText;
    public Text failText;
    public Text statsText;

    void Start()
    {
        gameObject.SetActive(false);
    }


    public void Win(float playTime)
    {
        Show(playTime);
        winText.gameObject.SetActive(true);

        PlayerProgress.levelUnlocked = level + 1;

        if (level < PlayerProgress.totalLevels)
            nextButton.gameObject.SetActive(true);
    }

    private void Show(float playTime)
    {
        level = int.Parse(SceneManager.GetActiveScene().name.Split('_')[1]);
        statsText.text = $"{Object.FindObjectsOfType<Infectable>().Count(p => p.species == Species.Friend) - 1} cells in {playTime:0.00}s";
        gameObject.SetActive(true);
    }

    public void Fail(float playTime)
    {
        Show(playTime);
        failText.gameObject.SetActive(true);
    }

    // Update is called once per frame
    public void Next()
    {
        SceneManager.LoadScene($"Level_{level + 1}");
    }

    public void Repeat()
    {
        SceneManager.LoadScene($"Level_{level}");
    }

    public void Exit()
    {
        SceneManager.LoadScene("Menu");
    }
}
