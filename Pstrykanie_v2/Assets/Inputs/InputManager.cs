using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : SingletonMonobehaviour<InputManager>
{
    private OhSnap inputs;

    public bool IsSnapping { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        inputs = new OhSnap();

        SubscripePlayerActions();

        inputs.Enable();
    }

    private void SubscripePlayerActions()
    {
        inputs.Player.Snap.performed += Snap_performed;
        inputs.Player.Snap.canceled += Snap_canceled;
    }

    private void Snap_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        IsSnapping = true;
    }

    private void Snap_canceled(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        IsSnapping = false;
    }
}
