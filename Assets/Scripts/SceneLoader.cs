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
    void Start()
    {
        Time.timeScale = 1;
        //audioSource = GetComponent<AudioSource>();
        currentLevel = SceneManager.GetActiveScene().buildIndex;
        
        nLevels = SceneManager.sceneCountInBuildSettings;
        //print("Level count :" + nLevels);
       // print("Current level : " + currentLevel);
        
        
    }

    // Update is called once per frame
    void Update()
    {

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
       
        int currentLevel = SceneManager.GetActiveScene().buildIndex;
       
        SceneManager.LoadScene(currentLevel);
    }

    public void ShowStartMenu()
    {
        SceneManager.LoadScene(0);
    }

    public int getLevelIndex()
    {
        return currentLevel;
    }
}
