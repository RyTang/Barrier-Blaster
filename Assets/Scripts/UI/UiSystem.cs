using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiSystem : MonoBehaviour
{

    [Header("UI Boards")]
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject endGameMenu;
    [SerializeField] private GameObject settingsMenu;


    [Header("Score UI")]
    [SerializeField] private TMPro.TextMeshProUGUI scoreText;
    [SerializeField] private TMPro.TextMeshProUGUI lifeText;
    [SerializeField] private CentralScore scoreSystem;


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

    /// <summary>
    /// Restarts the Game
    /// </summary>
    public void RestartGameButton(){
        GameManager.INSTANCE.OnRestartGame();
        pauseMenu.SetActive(false);
        endGameMenu.SetActive(false);
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
        pauseMenu.SetActive(false);
        endGameMenu.SetActive(false);
    }

    public void PauseGameButton(){
        GameManager.INSTANCE.OnPauseGame();
        pauseMenu.SetActive(true);
    }

    public void ContinueButton(){
        GameManager.INSTANCE.OnContinueGame();
        pauseMenu.SetActive(false);
        endGameMenu.SetActive(false);
    }

    public void ChangeMenuState(){
        // TODO: Create a function that automatically enables and disables the menu accordingly
    }
}
