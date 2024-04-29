using System;
using UnityEngine;

public class UserInput : MonoBehaviour
{
    private const string Horizontal = "Horizontal";

    private const KeyCode Run = KeyCode.LeftShift;
    private const KeyCode Jump = KeyCode.Space;
    private const KeyCode Attack = KeyCode.Mouse0;
    private const KeyCode UseHeal = KeyCode.Q;

    public Action<PlayerAnimatorData.LookDirection> DirectionChanged;
    public Action<PlayerAnimatorData.MoveState> SpeedChanged;
    public Action<bool> Jumped;
    public Action<bool> Attacked;
    public Action Healing;

    private void Update()
    {
        float horizontalValue = Input.GetAxisRaw(Horizontal);

        if (horizontalValue > 0)
        {
            DirectionChanged.Invoke(PlayerAnimatorData.LookDirection.Rigth);
            SpeedChanged.Invoke(Input.GetKey(Run) ? PlayerAnimatorData.MoveState.Run : PlayerAnimatorData.MoveState.Walk);
        }
        else if (horizontalValue < 0)
        {
            DirectionChanged.Invoke(PlayerAnimatorData.LookDirection.Left);
            SpeedChanged.Invoke(Input.GetKey(Run) ? PlayerAnimatorData.MoveState.Run : PlayerAnimatorData.MoveState.Walk);
        }
        else
        {
            SpeedChanged?.Invoke(PlayerAnimatorData.MoveState.Idle);
        }

        HoldDownButton(Attack, Attacked);
        HoldDownButton(Jump, Jumped);

        PressButton(UseHeal, Healing);
    }

    private void HoldDownButton(KeyCode key, Action<bool> action)
    {
        if (Input.GetKeyDown(key))
            action.Invoke(true);

        if (Input.GetKeyUp(key))
            action.Invoke(false);
    }

    private void PressButton(KeyCode key, Action unityAction)
    {
        if (Input.GetKeyDown(key))
            unityAction?.Invoke();
    }
}