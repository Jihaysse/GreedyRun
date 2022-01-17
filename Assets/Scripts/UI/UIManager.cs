using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    [SerializeField] MainMenuUI mainMenu;
    [SerializeField] Canvas levelMenu;
    [SerializeField] WinMenuUI winMenu;
    [SerializeField] LoseMenuUI loseMenu;

    void OnLevelStarted()
    {
        mainMenu.gameObject.SetActive(false);
        levelMenu.gameObject.SetActive(true);
    }

    void EnableWinMenu()
    {
        levelMenu.gameObject.SetActive(false);
        
        winMenu.Enable(true);

        winMenu.SetMenu(GameManager.Instance.Score, GameManager.Instance.ScoreMultiplier);
    }

    void EnableLoseMenu()
    {
        levelMenu.gameObject.SetActive(false);
        loseMenu.gameObject.SetActive(true);
    }

    void OnEnable()
    {
        GameManager.OnStartLevel += OnLevelStarted;
        Transporter.OnTransporterEnd += EnableWinMenu;
        Character.OnCharacterDead += EnableLoseMenu;

    }

    void OnDisable()
    {
        GameManager.OnStartLevel -= OnLevelStarted;
        Transporter.OnTransporterEnd -= EnableWinMenu;
        Character.OnCharacterDead -= EnableLoseMenu;
    }
}
