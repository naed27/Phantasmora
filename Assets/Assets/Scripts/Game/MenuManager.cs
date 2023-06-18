using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    // Managers
    [SerializeField] private SoundManager _soundManager;
    [SerializeField] private DungeonManager dungeonManager;

    // ---------------- UI
    [SerializeField] private Player _player;
    [SerializeField] private GameObject _pauseMenu;
    [SerializeField] private GameObject _mainMenu;
    [SerializeField] private GameObject _gameOverMenu;
    [SerializeField] private GameObject _victoryMenu;

    public void GoToMainMenu()
    {

        dungeonManager.DestroyDungeon();
        _player.Init();
        _player.HideStatusUI();

        HideAllExcept("main menu");
        _soundManager.PlayBackgroundMusic("mainMenu");

        Time.timeScale = 1;
    }


    public void PauseGame()
    {
        if (    _mainMenu.activeInHierarchy
            || _victoryMenu.activeInHierarchy
            ||  _gameOverMenu.activeInHierarchy 
           ) return;

        if (!_pauseMenu.activeInHierarchy)
        {
            Time.timeScale = 0;
            _pauseMenu.SetActive(true);
            _soundManager.PauseBackgroundMusic();
        }
        else
        {
            ResumeGame();
        }

        
    }

    public void ResumeGame()
    {

        HideAllExcept("none");

        Time.timeScale = 1;

        _soundManager.ResumeBackgroundMusic();
    }

    public void StartNewGame()
    {
        HideAllExcept("none");

        Time.timeScale = 1;

        dungeonManager.Init();

        _soundManager.PlayBackgroundMusic("inGame");
    }

    public void Gameover()
    {
        _player.HideStatusUI();
        HideAllExcept("gameover menu");

        Time.timeScale = 0;

    }
    public void Victory()
    {

        _player.HideStatusUI();
        HideAllExcept("victory menu");

        Time.timeScale = 0;

    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private void HideAllExcept(string menu)
    {
        _mainMenu.SetActive(menu == "main menu");
        _pauseMenu.SetActive(menu == "pause menu");
        _victoryMenu.SetActive(menu == "victory menu");
        _gameOverMenu.SetActive(menu == "gameover menu");
    }
}
