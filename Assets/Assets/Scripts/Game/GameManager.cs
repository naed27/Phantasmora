using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // ---------------- UI
    [SerializeField] private DungeonManager dungeonManager;
    [SerializeField] private GameObject _pauseMenu;
    [SerializeField] private GameObject _mainMenu;
    [SerializeField] private GameObject _gameOverMenu;
    [SerializeField] private GameObject _victoryMenu;

    public void GoToMainMenu()
    {
        _pauseMenu.SetActive(false);
        _victoryMenu.SetActive(false);
        _gameOverMenu.SetActive(false);

        dungeonManager.DestroyDungeon();
        dungeonManager.Player.Init();
        dungeonManager.Player.HideStatusBar();
        _mainMenu.SetActive(true);
        Time.timeScale = 1;
    }

    public void PauseGame()
    {
        if (    _mainMenu.activeInHierarchy
            || _pauseMenu.activeInHierarchy
            || _victoryMenu.activeInHierarchy
            ||  _gameOverMenu.activeInHierarchy 
           ) return;

        Time.timeScale = 0;

        _pauseMenu.SetActive(true);
    }

    public void ResumeGame()
    {
        _mainMenu.SetActive(false);
        _pauseMenu.SetActive(false);
        _victoryMenu.SetActive(false);
        _gameOverMenu.SetActive(false);

        Time.timeScale = 1;
    }

    public void RestartGame()
    {
        _mainMenu.SetActive(false);
        _pauseMenu.SetActive(false);
        _victoryMenu.SetActive(false);
        _gameOverMenu.SetActive(false);

        Time.timeScale = 1;

        dungeonManager.Init();
    }

    public void Gameover()
    {
        _mainMenu.SetActive(false);
        _pauseMenu.SetActive(false);
        _victoryMenu.SetActive(false);

        Time.timeScale = 0;

        _gameOverMenu.SetActive(true);
    }
    public void Victory()
    {
        _mainMenu.SetActive(false);
        _pauseMenu.SetActive(false);
        _gameOverMenu.SetActive(false);

        Time.timeScale = 0;

        _victoryMenu.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
