using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class SpeechBubble : MonoBehaviour
{
    [SerializeField] TextMeshPro priceText;
    [SerializeField] float timeToPopUp = 0.5f;
    [SerializeField] BoxCollider coll;
    [SerializeField] List<Transform> childrens;
    [SerializeField] AudioSource popupSound;

    Vector3 scale;
    float yPosition;

    void Start()
    {
        // Cache scale and yPos for later use when calling PopUp()
        scale = transform.localScale;
        yPosition = transform.localPosition.y;

        // Deactivate
        SetActive(false);
    }

    public void SetPriceText(int price)
    {
        priceText.text = price.ToString();
    }

    void SetActive(bool isActive)
    {
        for (int i = 0; i < childrens.Count; i++)
        {
            childrens[i].gameObject.SetActive(isActive);
        }
    }

    void PopUp()
    {
        SetActive(true);
        transform.localScale = Vector3.zero;
        transform.localPosition = new Vector3(transform.localPosition.x, 0, transform.localPosition.z);

        transform.DOScale(scale, timeToPopUp);
        transform.DOLocalMoveY(yPosition, timeToPopUp);

        popupSound?.Play();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 6)
        {
            coll.enabled = false;
            PopUp();
        }
    }

    void OnValidate()
    {
        if (coll == null)
            coll = GetComponent<BoxCollider>();
    }
}
