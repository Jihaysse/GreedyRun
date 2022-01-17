using UnityEngine;
using TMPro;
using DG.Tweening;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI tapText;

    void Start()
    {
        SetTapTextBouncing(true);
    }

    void SetTapTextBouncing(bool grow)
    {
        if (grow)
            tapText.transform.DOScale(new Vector3(1.1f, 1.1f, 1.1f), 1f).OnComplete( () => SetTapTextBouncing(false));
        else
            tapText.transform.DOScale(new Vector3(.9f, .9f, .9f), 1f).OnComplete( () => SetTapTextBouncing(true));
    }
}
