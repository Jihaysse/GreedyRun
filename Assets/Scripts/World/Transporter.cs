using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transporter : MonoBehaviour
{
    [SerializeField] float speedMultiplier = 2f;

    [Tooltip("Delay before popping an item from the player's stack")]
    [SerializeField] [Range(0.1f, 2f)] float timeToPopItem = 0.5f;
    [SerializeField] ParticleSystem leftDollarTrail;
    [SerializeField] ParticleSystem rightDollarTrail;

    WaitForSeconds delay;

    public static event Action OnTransporterEnd;

    void Start()
    {
        delay = new WaitForSeconds(timeToPopItem);
    }

    public IEnumerator UseTransporter(Character user)
    {
        PlayerController player = user.GetComponent<PlayerController>();
        if (player != null)
        {
            // play VFX
            EnableTrail(true);

            // Set vehicle to follow player
            transform.SetParent(player.transform);
            transform.position = player.transform.position;
            transform.rotation = player.transform.rotation;

            user.SetRunningAnimation(false);

            player.BoostSpeed(speedMultiplier);
            player.SetGravityActive(false);
            
            while (user.ItemStack.Size > 0)
            {
                user.ItemStack.PopGameObject();
                yield return delay;
            }

            // End
            player.StopMoving();
            EnableTrail(false);

            OnTransporterEnd?.Invoke();
        }
    }

    void EnableTrail(bool enable)
    {
        leftDollarTrail?.gameObject.SetActive(enable);
        rightDollarTrail?.gameObject.SetActive(enable);
    }




}
