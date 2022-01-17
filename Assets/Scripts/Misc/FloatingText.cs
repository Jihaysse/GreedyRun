using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
public class FloatingText : MonoBehaviour
{
    [SerializeField] TextMeshPro textMesh;
    [SerializeField] [Range(0.1f, 3f)] float timeToPopup = .5f;
    [SerializeField] [Range(0.1f, 3f)] float timeToDisappear = .5f;
    
    Vector3 textScale;
    float endYpos;
    Coroutine anim;
    WaitForSeconds delayBeforeDisappearing;

    void Start()
    {
        textScale = transform.localScale;
        endYpos = transform.localPosition.y;
        delayBeforeDisappearing = new WaitForSeconds(timeToDisappear);

        SetText(string.Concat("+" + Money.MONEY_VALUE.ToString()));
    }

    void SetText(string text)
    {
        textMesh.text = text;
    }

    void PopUp()
    {
        if (anim != null)
            StopCoroutine(anim);

        textMesh.enabled = true;
        transform.localScale = Vector3.zero;
        transform.DOLocalMoveY(0, 0.01f);

        anim = StartCoroutine(Animate());        
    }

    IEnumerator Animate()
    {
        transform.DOScale(textScale, timeToPopup);
        yield return transform.DOLocalMoveY(endYpos, timeToPopup).WaitForCompletion();
        yield return delayBeforeDisappearing;
        transform.DOScale(Vector3.zero, .2f);
    }

    void OnValidate()
    {
        if (textMesh == null)
            textMesh = GetComponent<TextMeshPro>();
    }

    void OnEnable()
    {
        Character.OnMoneyCollected += PopUp;
    }

    void OnDisable()
    {
        Character.OnMoneyCollected -= PopUp;
    }
    
}
