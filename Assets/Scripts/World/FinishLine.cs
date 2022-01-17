using System;
using System.Collections.Generic;
using UnityEngine;

public class FinishLine : MonoBehaviour
{
    [SerializeField] List<ParticleSystem> vfxList;
    [SerializeField] AudioSource winAudio;

    public static event Action OnFinishLineCross;

    void CrossFinishLine()
    {
        // Play all VFX
        for (int i = 0; i < vfxList.Count; i++)
        {
            vfxList[i].Play();
        }

        winAudio?.Play();
        OnFinishLineCross?.Invoke();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 6) // Character
        {
            CrossFinishLine();
        }
    }
}
