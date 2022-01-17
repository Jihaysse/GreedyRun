using System;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.NiceVibrations;

public class Character : MonoBehaviour
{
    [SerializeField] PlayerController player;
    [SerializeField] Animator animator;
    [SerializeField] ItemStack itemStack;
    [SerializeField] List<Collider> ragdollColliders;
    [SerializeField] List<Rigidbody> ragdollRigidbodies;
    [SerializeField] AudioSource dieSound;

    int currentMoney = 0;

    public static event Action OnCharacterDead;
    public static event Action OnMoneyCollected;

    public ItemStack ItemStack => itemStack;

    void CollectMoney(Money money)
    {
        currentMoney += money.Amount;
        itemStack.StackGameObject(money.gameObject);
        MMVibrationManager.Haptic(HapticTypes.Selection); // vibrate
        OnMoneyCollected?.Invoke();
    }

    void SpendMoney(int amount)
    {
        if (currentMoney < amount) return;

        currentMoney -= amount;
        MMVibrationManager.Haptic(HapticTypes.Failure);

        // disable the gameObjects on the stack
        while (amount >= Money.MONEY_VALUE)
        {
            itemStack.PopGameObject();
            amount -= Money.MONEY_VALUE;
        }        
    }

    bool HasEnoughMoney(int priceToPay)
    {
        if (currentMoney >= priceToPay) 
            return true;
        
        return false;
    }

    void ActivateRagdoll()
    {
        for (int i = 0; i < ragdollColliders.Count; i++)
        {
            ragdollColliders[i].enabled = true;
            ragdollRigidbodies[i].isKinematic = false;
        }
    }

    void Die()
    {
        ActivateRagdoll();
        animator.enabled = false;
        player.StopMoving();
        dieSound?.Play();
        MMVibrationManager.Haptic(HapticTypes.HeavyImpact);

        OnCharacterDead?.Invoke();
    }

    public void SetRunningAnimation(bool isRunning)
    {
        animator.SetBool("Run", isRunning);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 7) // money
        {
            Money moneyItem = other.GetComponent<Money>();
            if (moneyItem != null)
            {
                moneyItem.OnPickedUp();
                CollectMoney(moneyItem);
            }
        }
        else if (other.gameObject.layer == 8) // obstacle
        {
            Obstacle obstacle = other.GetComponent<Obstacle>();
            if (obstacle != null)
            {
                if (HasEnoughMoney(obstacle.Price))
                {
                    // Pay the price
                    SpendMoney(obstacle.Price);
                    // Deactivate obstacle
                    obstacle.OnTriggerObstacle();
                }
                else
                {
                    // Lose as player doesn't have enough money to pay for the obstacle
                    Die();
                }
            }
        }
        else if (other.gameObject.layer == 9) // transporter
        {
            Transporter transporter = other.GetComponent<Transporter>();
            if (transporter != null)
            {
                StartCoroutine(transporter.UseTransporter(this));
            }
        }
        else if (other.gameObject.layer == 4) // Water
        {
            Die();
        }
    }

    void OnValidate()
    {
        if (player == null)
        {
            player = GetComponent<PlayerController>();
        }
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
        if (itemStack == null)
        {
            itemStack = GetComponentInChildren<ItemStack>();
        }
    }
}
