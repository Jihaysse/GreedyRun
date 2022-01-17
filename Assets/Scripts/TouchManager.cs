using System;
using UnityEngine;
using DigitalRubyShared;

public class TouchManager : MonoBehaviour
{
    [SerializeField] FingersJoystickScript joystick;
    [SerializeField] PlayerController player;

    TapGestureRecognizer tapGesture;
    public static event Action OnTapGesture;

    void Start()
    {
        joystick.JoystickExecuted = OnJoystickExecuted;
    }

    void TapGestureFired(GestureRecognizer tap)
    {
        if (tap.State == GestureRecognizerState.Ended)
        {
            OnTapGesture?.Invoke();
        }
    }

    void OnJoystickExecuted(FingersJoystickScript script, Vector2 amount)
    {
        if (player != null)
        {
            player.Move(amount.x);
        }
    }

    void OnEnable()
    {
        tapGesture = new TapGestureRecognizer();
        tapGesture.ClearTrackedTouchesOnEndOrFail = true;
        tapGesture.StateUpdated += TapGestureFired;
        tapGesture.AllowSimultaneousExecutionWithAllGestures();
        FingersScript.Instance.AddGesture(tapGesture);
    }

    void ValidateReferences()
    {
        if (player == null)
        {
            player = FindObjectOfType<PlayerController>();
        }

        if (joystick == null)
        {
            joystick = FindObjectOfType<FingersJoystickScript>();
        }
    }

    void OnValidate()
    {
        
    }
}
