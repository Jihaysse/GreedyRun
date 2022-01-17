using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ScoreBooster : MonoBehaviour
{
    [SerializeField] [Range(2, 20)] int multiplier = 2;
    [SerializeField] AudioSource winAudio;
    [SerializeField] Transform wall;

    Vector3 wallScale;

    void Start()
    {
        wallScale = wall.localScale; 
    }

    void ActivateWall()
    {
        wall.gameObject.SetActive(true);

        // Animate
        wall.localScale = Vector3.zero;
        wall.DOScale(wallScale, 1f);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 6) // Character
        {
            GameManager.Instance.SetScoreMultiplier(multiplier);
            winAudio?.Play();
        }
    }

    void OnEnable()
    {
        FinishLine.OnFinishLineCross += ActivateWall;
    }

    void OnDisable()
    {
        FinishLine.OnFinishLineCross -= ActivateWall;
    }

}
