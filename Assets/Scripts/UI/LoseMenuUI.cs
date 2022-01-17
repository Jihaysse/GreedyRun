using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoseMenuUI : MonoBehaviour
{
    public void OnTryAgainButtonClicked()
    {
        GameManager.Instance.ReloadScene();
    }
}
