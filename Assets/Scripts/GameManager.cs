using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager INSTANCE;
    
    public CentralScore scoreSystem;

    [Header("Manager Events")]
    [SerializeField] private GameEvent pauseGameEvent;
    [SerializeField] private GameEvent endGameEvent;
    [SerializeField] private GameEvent restartGameEvent;
    [SerializeField] private GameEvent startGameEvent;
    [SerializeField] private GameEvent returnToStartMenuEvent;

    void Start()
    {
        // Create instance object
        if (INSTANCE != null){
            Destroy(gameObject);
            return;
        }

        INSTANCE = this;
        DontDestroyOnLoad(this.gameObject);
    }

    public void FixedUpdate()
    {
        // Update the corresponding Time Value
        scoreSystem.SetDurationSurvived(scoreSystem.GetDurationSurvived() + Time.fixedDeltaTime);
    }

    // SCENE LOADER


    // EVENT TRIGGERER

    /// <summary>
    /// Will be triggered on Player's Death, any objects that needs to be triggered upon player's death should be tied to this event
    /// </summary>
    public void OnPlayerDeath(){
        // playerDeathEvent.TriggerEvent();
        endGameEvent.TriggerEvent();

        Time.timeScale = 0f;
    }

    /// <summary>
    /// Will be triggered on Player's Death, any objects that needs to be triggered upon player's death should be tied to this event
    /// </summary>
    public void OnRestartGame()
    {
        UpdateProgress();
        restartGameEvent.TriggerEvent();

        // Load Scene
        SceneManager.LoadScene((int)SceneName.GAME);


        Time.timeScale = 1f;
    }

    private void UpdateProgress()
    {
        GameData gameData = SaveSystem.INSTANCE.LoadGameData();

        if (gameData is null)
        {
            gameData = new GameData();
        }

        gameData.UpdateGameData(scoreSystem);

        scoreSystem.ResetScoreSystem();

        SaveSystem.INSTANCE.SaveGameData(gameData);
    }

    public void OnQuitGame(){
        UpdateProgress();
    }

    public void OnReturnToStartMenu(){
        UpdateProgress();
        returnToStartMenuEvent.TriggerEvent();

        SceneManager.LoadScene((int) SceneName.START_MENU);
    }

    /// <summary>
    /// Any Objects that needs to be reset should be listening to the Start Game Event
    /// </summary>
    public void OnStartGame(){

        // TODO: Load Data Points and distribute the data accordingly
        GameData gameData = SaveSystem.INSTANCE.LoadGameData();
        startGameEvent.TriggerEvent();

        // Start Game
        SceneManager.LoadScene((int) SceneName.GAME);

        Time.timeScale = 1;
    }

    /// <summary>
    /// Called when needing to Pause the Game Logic properly, will possibly let other instances run in the back
    /// </summary>
    public void OnPauseGame(){
        Time.timeScale = 0f;
    }

    /// <summary>
    /// Called when needing to Continue the Game Logic properly, will possibly let other instances run in the back
    /// </summary>
    public void OnContinueGame(){
        Time.timeScale = 1f;
    }
}
