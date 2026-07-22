using System;
using UnityEngine;
using UnityEngine.InputSystem;
using static Controls;

[CreateAssetMenu(fileName = "NewInputReader", menuName = "Input/Input Reader")]
public class InputReader: ScriptableObject, IPlayerActions
{
    public event Action<bool> PrimaryFireEvent;
    public event Action<bool> SecondaryFireEvent;
    public event Action<Vector2> MoveEvent;
    
    public Vector2 AimPosition { get; private set; }

    private Controls _controls;
    private void OnEnable()
    {
        if (_controls == null)
        {
            _controls = new Controls();
            _controls.Player.SetCallbacks(this);
        }
        _controls.Player.Enable();
    }

    private void OnDisable()
    {
        _controls.Player.Disable();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
       MoveEvent?.Invoke(context.ReadValue<Vector2>());
    }

    public void OnPrimaryAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            PrimaryFireEvent?.Invoke(true);
        }
        else if (context.canceled)
        {
            PrimaryFireEvent?.Invoke(false);
        }
    }

    public void OnSecondaryAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            SecondaryFireEvent?.Invoke(true);
        }
        else if (context.canceled)
        {
            SecondaryFireEvent?.Invoke(false);
        }
    }

    public void OnAim(InputAction.CallbackContext context)
    {
        AimPosition = context.ReadValue<Vector2>();
    }
}
