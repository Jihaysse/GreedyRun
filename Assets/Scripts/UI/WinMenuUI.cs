using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class WinMenuUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI scoreMultiplierText;
    [SerializeField] TextMeshProUGUI finalScoreText;
    [SerializeField] Transform panel;

    public void SetMenu(int score, int scoreMultiplier)
    {
        scoreText.text = string.Concat("Score: " + score);
        scoreMultiplierText.text = string.Concat("Multiplier: x" + scoreMultiplier);

        int finalScore = score * scoreMultiplier;
        finalScoreText.text = finalScore.ToString();
    }
    
    public void Enable(bool isActive)
    {
        gameObject.SetActive(isActive);

        if (isActive)
        {
            // Animate
            panel.localScale = Vector3.zero;
            panel.DOScale(Vector3.one, .5f);
        }
    }

    public void OnNextButtonClicked()
    {
        GameManager.Instance.ReloadScene();
    }
}
