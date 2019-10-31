using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneLoader : MonoBehaviour
{
    // Start is called before the first frame update
    int currentLevel;
    int nLevels;

    [SerializeField] WinScreen winScreen;
    private void Awake()
    {
        currentLevel = SceneManager.GetActiveScene().buildIndex;
    }
    void Start()
    {
        Time.timeScale = 1f;
        nLevels = SceneManager.sceneCountInBuildSettings;
         
    }


    public void GameOver()
    {
        print("Gameover");
        // Destroy rocket ship, make sound effect, show game over text an UI menu to restart or quit or menu

    }
    public void LoadFirstLevel()
    {

        SceneManager.LoadScene(1);
    }

    public void LoadNextLevel()
    {
       if (currentLevel+1 < nLevels)
        {
        SceneManager.LoadScene(currentLevel + 1);

        }
       else
        {
            winScreen.ShowWinScreen();
     
        }
    }
    public void RestartLevel()
    {
        print("Restartlevle");
        int currentLevel = SceneManager.GetActiveScene().buildIndex;
       
        SceneManager.LoadScene(currentLevel);
    }

    public void ShowStartMenu()
    {
        SceneManager.LoadScene(0);
    }

    public int getLevelIndex()
    {
        currentLevel = SceneManager.GetActiveScene().buildIndex;
        return currentLevel;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
