using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;

public class AnimatorHandler : MonoBehaviour
{
    [SerializeField] private int _groundLayerIndex = 1;
    [SerializeField] private int _flyLayerIndex = 2;

    private SpriteRenderer _sprite;
    private PlayerInventory _playerInventary;
    private Animator _animator;

    private Dictionary<int, PlayerAnimatorData.MoveState> _moveStates;
    private Dictionary<int, PlayerAnimatorData.VerticalState> _verticalStates;

    public UnityAction AttackCompleted;
    public UnityAction HealingCompleted;

    private void Start()
    {
        _moveStates = new Dictionary<int, PlayerAnimatorData.MoveState>
        {
            { PlayerAnimatorData.Params.Idle, PlayerAnimatorData.MoveState.Idle },
            { PlayerAnimatorData.Params.Walk, PlayerAnimatorData.MoveState.Walk },
            { PlayerAnimatorData.Params.Run, PlayerAnimatorData.MoveState.Run }
        };

        _verticalStates = new Dictionary<int, PlayerAnimatorData.VerticalState>
        {
            { PlayerAnimatorData.Params.Ground, PlayerAnimatorData.VerticalState.Ground },
            { PlayerAnimatorData.Params.Jump, PlayerAnimatorData.VerticalState.DoesTopMove },
            { PlayerAnimatorData.Params.Fall , PlayerAnimatorData.VerticalState.Fall }
        };
    }

    public void Initialization(Animator animator, SpriteRenderer spriteRenderer, PlayerInventory playerInventory)
    {
        _animator = animator;
        _sprite = spriteRenderer;
        _playerInventary = playerInventory;
    }

    public void Subscribe(UserInput userInput, Jumper jumper, Health battleHandler)
    {
        userInput.Attacked += CheckAttack;
        userInput.Healing += CheckHealing;
        userInput.SpeedChanged += CheckHorizontalPosition;
        userInput.DirectionChanged += Look2;

        jumper.StateChanged += CheckVerticalPosition;

        battleHandler.Die += CheckDie;
    }

    public void Unsubscribe(UserInput userInput, Jumper jumper, Health battleHandler)
    {
        userInput.Attacked -= CheckAttack;
        userInput.Healing -= CheckHealing;
        userInput.SpeedChanged -= CheckHorizontalPosition;
        userInput.DirectionChanged -= Look2;

        jumper.StateChanged -= CheckVerticalPosition;

        battleHandler.Die -= CheckDie;
    }

    private void Look2(PlayerAnimatorData.LookDirection lookDirection)
    {
        _sprite.flipX = lookDirection == PlayerAnimatorData.LookDirection.Left;
    }

    private void CheckVerticalPosition(PlayerAnimatorData.VerticalState state)
    {
        foreach (int key in _verticalStates.Keys)
        {
            _animator.SetBool(key, _verticalStates[key] == state);
        }

        if (state == PlayerAnimatorData.VerticalState.Ground)
        {
            _animator.SetLayerWeight(_groundLayerIndex, 1f);
            _animator.SetLayerWeight(_flyLayerIndex, 0f);
        }
        else
        {
            _animator.SetLayerWeight(_groundLayerIndex, 0f);
            _animator.SetLayerWeight(_flyLayerIndex, 1f);
        }
    }

    private void CheckHorizontalPosition(PlayerAnimatorData.MoveState moveState)
    {
        foreach (int key in _moveStates.Keys)
        {
            _animator.SetBool(key, _moveStates[key] == moveState);
        }
    }

    private void CheckDie()
    {
        _animator.SetTrigger(PlayerAnimatorData.Params.Dead);
    }

    private void CheckAttack(bool atakked)
    {
        _animator.SetBool(PlayerAnimatorData.Params.SwordAttack, atakked);
    }

    private void CheckHealing()
    {
        if (_playerInventary.TryHeal)
            _animator.SetTrigger(PlayerAnimatorData.Params.Buff2);
    }

    private void AcceptAttack()
    {
        HealingCompleted.Invoke();
    }

    private void AcceptHealing()
    {
        AttackCompleted.Invoke();
    }
}