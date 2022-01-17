using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Obstacle : MonoBehaviour
{
    [SerializeField] SpeechBubble speechBubble;
    [SerializeField] ParticleSystem dollarBlastVFX;
    [SerializeField] int price = 500;
    
    [Tooltip("Scale multiplier when triggered")]
    [SerializeField] [Range(1f, 2f)] float scaleMultiplier = 1.5f;

    [Tooltip("Delay in seconds to grow before shrinking")]
    [SerializeField] [Range(0.1f, 1f)] float timeToGrow = 0.2f;
    [SerializeField] [Range(0.1f, 1f)] float timeToShrink = 0.5f;

    public int Price => price;

    void Start()
    {
        speechBubble.SetPriceText(price);
    }

    public void OnTriggerObstacle()
    {
        if (dollarBlastVFX != null)
        {
            dollarBlastVFX.transform.SetParent(null);
            dollarBlastVFX.Play();
        }
            

        StartCoroutine(GrowThenDisappear());
    }

    IEnumerator GrowThenDisappear()
    {
        float scale = transform.localScale.x; // we assume scale is uniform

        yield return transform.DOScale(new Vector3(scale * scaleMultiplier, scale * scaleMultiplier, scale * scaleMultiplier), timeToGrow).WaitForCompletion();
        yield return transform.DOScale(Vector3.zero, timeToShrink).WaitForCompletion();
        gameObject.SetActive(false);
    }

    void OnValidate()
    {
        if (speechBubble == null)
        {
            speechBubble = GetComponentInChildren<SpeechBubble>();
        }
        if (dollarBlastVFX == null)
        {
            dollarBlastVFX = GetComponentInChildren<ParticleSystem>();
        }
    }

}
