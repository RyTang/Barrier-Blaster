using System.Diagnostics;
using System.Linq.Expressions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiSystem : MonoBehaviour
{
    [Header("UI Boards")]
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject endGameMenu;
    [SerializeField] private GameObject settingsMenu;
    [SerializeField] private GameObject startMenu;

    [Header("Blocker Screen")]
    [SerializeField] private GameObject blockerScreen;


    [Header("Score UI")]
    [SerializeField] private TMPro.TextMeshProUGUI scoreText;
    [SerializeField] private TMPro.TextMeshProUGUI lifeText;
    [SerializeField] private CentralScore scoreSystem;

    private UIState currentState;


    void Start()
    {
        UpdateScoreUI();
        UpdateLifeUI();
    }

    /// <summary>
    /// Updates UI of Score with current score
    /// </summary>
    public void UpdateScoreUI(){
        scoreText.text = scoreSystem.GetScoreAmount().ToString();
    }

    /// <summary>
    /// Updates UI of Life with current life
    /// </summary>
    public void UpdateLifeUI(){
        lifeText.text = scoreSystem.GetLifeAmount().ToString();
    }

    public void ShowEndGameMenu(){
        ChangeMenuState(UIState.END_MENU);
    }

    /// <summary>
    /// Restarts the Game
    /// </summary>
    public void RestartGameButton(){
        GameManager.INSTANCE.OnRestartGame();
        ChangeMenuState(UIState.RUNNING_GAME);
    }

    /// <summary>
    /// Restarts the Game
    /// </summary>
    public void QuitGameButton() {
        GameManager.INSTANCE.OnQuitGame();
        Application.Quit();
    }

    /// <summary>
    /// Starts the Game
    /// </summary>
    public void StartGameButton(){
        GameManager.INSTANCE.OnStartGame();
        ChangeMenuState(UIState.RUNNING_GAME);
    }

    public void PauseGameButton(){
        GameManager.INSTANCE.OnPauseGame();
        ChangeMenuState(UIState.PAUSE_MENU);
    }

    public void ContinueButton(){
        GameManager.INSTANCE.OnContinueGame();
        ChangeMenuState(UIState.RUNNING_GAME);
    }

    public void ReturnToStartButton(){
        GameManager.INSTANCE.OnReturnToStartMenu();
        ChangeMenuState(UIState.START_MENU);
    }

    /// <summary>
    /// Used to handle the state changes between menues, detailing what menus should be opened or closed
    /// </summary>
    /// <param name="stateToChangeTo">States to change into</param>
    public void ChangeMenuState(UIState stateToChangeTo){
        // TODO: Create a function that automatically enables and disables the menu accordingly
        if (currentState == stateToChangeTo) return;

        switch (stateToChangeTo){
            case UIState.RUNNING_GAME:
                pauseMenu.SetActive(false);
                endGameMenu.SetActive(false);
                blockerScreen.SetActive(false);
                startMenu.SetActive(false);
                break;
            case UIState.PAUSE_MENU:
                pauseMenu.SetActive(true);
                blockerScreen.SetActive(true);
                startMenu.SetActive(false);
                break;
            case UIState.END_MENU:
                pauseMenu.SetActive(false);
                endGameMenu.SetActive(true);
                blockerScreen.SetActive(true);
                startMenu.SetActive(false);
                break;
            case UIState.START_MENU:
                pauseMenu.SetActive(false);
                endGameMenu.SetActive(false);
                blockerScreen.SetActive(false);
                startMenu.SetActive(false);
                break;
            default:
                break;
        }

        currentState = stateToChangeTo;
    }
}

public enum UIState {
        RUNNING_GAME,
        PAUSE_MENU,
        SETTINGS_MENU,
        END_MENU,
        START_MENU
    }
