using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    [SerializeField] string _sceneRestart;
    [SerializeField] string _sceneMainMenu;

    public static bool isGameOver;

    public void SetUp()
    {

    }

    public void Restart()
    {
        SceneManager.LoadScene(_sceneRestart);
        isGameOver = false;
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(_sceneMainMenu);
        isGameOver = false;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
