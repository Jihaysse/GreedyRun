using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Money : MonoBehaviour
{
    [SerializeField] BoxCollider coll;
    [SerializeField][Range(50, 300)] int amount = 100;
    [SerializeField] AudioSource moneySound;
    [SerializeField] ParticleSystem activateVFX;

    public static int MONEY_VALUE { get; private set; } // for ease of access and to make sure nothing breaks if we change money's amount

    void Awake()
    {
        MONEY_VALUE = amount;
    }

    public int Amount => amount;

    public void OnPickedUp()
    {
        // Disable collider so it doesn't get triggered again
        coll.enabled = false;

        moneySound.Play();
        
        activateVFX.transform.SetParent(null);
        activateVFX.Play();
    }

    void OnValidate()
    {
        if (coll == null)
        {
            coll = GetComponent<BoxCollider>();
        }
        if (moneySound == null)
        {
            moneySound = GetComponent<AudioSource>();
        }
    }
}
