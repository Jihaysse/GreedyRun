using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region Singleton
    static GameManager instance;
    public static GameManager Instance => instance;
    #endregion
    enum GameState
    {
        MAIN_MENU,
        PLAYING,
        LEVEL_END
    }

    [SerializeField][Range(20, 120)] int targetFPS = 60;

    [Tooltip("The value which will increase the score every time the player picks an item up")]
    [SerializeField] int scoreBoost = 10;
    
    GameState state = GameState.MAIN_MENU;
    public int Score { get; private set; }
    public int ScoreMultiplier { get; private set; }

    public static event Action OnStartLevel;
    public static event Action<bool> OnEndLevel;

    void Awake()
    {
        #region Singleton Initialization
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
        #endregion
    }

    void Start()
    {
        Application.targetFrameRate = targetFPS;
    }

    void StartLevel()
    {
        if (state != GameState.MAIN_MENU) return;

        state = GameState.PLAYING;
        OnStartLevel?.Invoke();
    }

    // WinLevel and LoseLevel aren't used in this prototype but can be used in a real-world game (to increment current level, save data, etc...)
    void WinLevel()
    {
        state = GameState.LEVEL_END;
    }

    void LoseLevel()
    {
        state = GameState.LEVEL_END;
    }

    void UpdateScore()
    {
        Score += scoreBoost;
    }

    public void ReloadScene()
    {
        // looping level
        SceneManager.LoadScene(0);
    }

    public void SetScoreMultiplier(int multiplier)
    {
        ScoreMultiplier = multiplier;
    }

    void OnEnable()
    {
        Transporter.OnTransporterEnd += WinLevel;
        Character.OnCharacterDead += LoseLevel;
        Character.OnMoneyCollected += UpdateScore;
        TouchManager.OnTapGesture += StartLevel;
    }

    void OnDisable()
    {
        Transporter.OnTransporterEnd -= WinLevel;
        Character.OnCharacterDead -= LoseLevel;
        Character.OnMoneyCollected -= UpdateScore;
        TouchManager.OnTapGesture -= StartLevel;
    }
}
