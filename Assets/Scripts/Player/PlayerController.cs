using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DigitalRubyShared;
using System;

[RequireComponent(typeof(Animator), typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Tooltip("The speed at which the player will move using the joystick")] 
    [SerializeField] [Range(1f, 20f)] float moveSpeed = 5f;

    [Tooltip("The speed at which the player will run forward")]
    [SerializeField] [Range(1f, 20f)] float runningSpeed = 5f;
    [SerializeField] bool applyGravity = true;
    [SerializeField] float gravityValue = -9.81f;

    [SerializeField] Character character;
    [SerializeField] CharacterController characterController;
    
    float baseSpeed;
    Vector3 gravity;
    bool canMove = false;

    void Awake()
    {
        baseSpeed = runningSpeed;
        gravity = new Vector3(0, gravityValue, 0);
    }
    

    void Update()
    {
        RunForward();

        if (applyGravity)
            ApplyGravity();
    }


    /// <summary>Move the player sideways. Called by TouchManager when joystick is executed. </summary>
    public void Move(float joystickHorizontal)
    {
        if (!canMove) return;

        Vector3 direction = new Vector3(joystickHorizontal, 0, 0);
        characterController.Move(direction * moveSpeed * Time.deltaTime);
    }

    void RunForward()
    {
        if (!canMove) return;

        characterController.Move(transform.forward * runningSpeed * Time.deltaTime);
    }

    void ApplyGravity()
    {
        characterController.Move(gravity * Time.deltaTime);
    }

    public void SetGravityActive(bool active)
    {
        applyGravity = active;
    }

    public void BoostSpeed(float multiplier)
    {
        runningSpeed *= multiplier;
    }

    public void ResetSpeed()
    {
        runningSpeed = baseSpeed;
    }

    public void StopMoving()
    {
        canMove = false;
        character.SetRunningAnimation(false);
    }

    void OnLevelStarted()
    {
        canMove = true;
        character?.SetRunningAnimation(true);
    }

    void OnValidate()
    {
        if (character == null)
            character = GetComponent<Character>();

        if (characterController == null)
            characterController = GetComponent<CharacterController>();
    }

    private void OnEnable()
    {
        GameManager.OnStartLevel += OnLevelStarted;
    }

    private void OnDisable()
    {
        GameManager.OnStartLevel -= OnLevelStarted;
    }
    
}
