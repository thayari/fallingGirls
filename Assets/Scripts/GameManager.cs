using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public enum GameState
    {
        MainMenu,
        Playing,
        Paused,
        GameOver
    }

    public GameState currentState;

    [SerializeField] private Movement playerMovement; 
    [SerializeField] private UIManager uiManager;

    [SerializeField] private float _restartTimer = 1f;

    public UnityEvent OnGameStart;
    public UnityEvent OnGameOver;


    void Start()
    {
        ChangeState(GameState.MainMenu);
    }

    public void ChangeState(GameState newState)
    {
        currentState = newState;

        switch (currentState)
        {
            case GameState.MainMenu:
                Time.timeScale = 1;
                playerMovement.enabled = false;
                //uiManager.ShowMainMenu();
                break;

            case GameState.Playing:
                Time.timeScale = 1;
                playerMovement.enabled = true;
                //uiManager.ShowGameHUD();
                OnGameStart.Invoke();
                Debug.Log("Start");
                break;

            case GameState.Paused:
                Time.timeScale = 0; 
                playerMovement.enabled = false;
                //uiManager.ShowPauseMenu();
                break;

            case GameState.GameOver:
                //uiManager.ShowGameOverScreen();
                OnGameOver.Invoke();
                StartCoroutine(RestartSceneAfterDelay());
                break;
        }
    }

    // Публичные методы, которые будут вызываться извне
    public void StartGame()
    {
        ChangeState(GameState.Playing);
    }

    public void PlayerDied()
    {
        if (currentState == GameState.Playing) 
        {
            ChangeState(GameState.GameOver);
        }
    }

    private IEnumerator RestartSceneAfterDelay()
    {
        // Ждем указанное количество времени
        yield return new WaitForSeconds(_restartTimer);

        // Перезагружаем текущую сцену
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
    }
}