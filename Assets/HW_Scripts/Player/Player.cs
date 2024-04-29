using UnityEngine;

[RequireComponent(typeof(UserInput))]
[RequireComponent(typeof(Mover))]
[RequireComponent(typeof(Jumper))]
[RequireComponent(typeof(AnimatorHandler))]
[RequireComponent(typeof(Health))]
[RequireComponent(typeof(AttackType))]
[RequireComponent(typeof(PlayerInventory))]

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    private UserInput _userInput;
    private Mover _mover;
    private Jumper _jumper;
    private AnimatorHandler _animatorHandler;
    private Health _battleHandler;
    private AttackType _attackType;
    private PlayerInventory _playerInventery;

    private Animator _animator;
    private SpriteRenderer _spriteRenderer;
    private Rigidbody2D _rigidbody;

    public Health Health => _battleHandler;

    private void Start()
    {
        _userInput = GetComponent<UserInput>();
        _mover = GetComponent<Mover>();
        _jumper = GetComponent<Jumper>();
        _animatorHandler = GetComponent<AnimatorHandler>();
        _battleHandler = GetComponent<Health>();
        _attackType = GetComponent<AttackType>();
        _playerInventery = GetComponent<PlayerInventory>();

        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _rigidbody = GetComponent<Rigidbody2D>();

        _mover.Initialization(_jumper);
        _jumper.Initialization(_rigidbody);
        _animatorHandler.Initialization(_animator, _spriteRenderer, _playerInventery);
        _playerInventery.Initialization(_battleHandler);

        _mover.Subscribe(_userInput);
        _jumper.Subscribe(_userInput);
        _animatorHandler.Subscribe(_userInput, _jumper, _battleHandler);
        _attackType.Subscribe(_userInput, _animatorHandler);
        _playerInventery.Subscribe(_animatorHandler);
    }

    private void OnDisable()
    {
        _mover.Unsubscribe(_userInput);
        _jumper.Unsubscribe(_userInput);
        _animatorHandler.Unsubscribe(_userInput, _jumper, _battleHandler);
        _attackType.Unsubscribe(_userInput);
        _playerInventery.Unsubscribe(_animatorHandler);
    }

    
}

public static class PlayerAnimatorData
{
    public static class Params
    {
        public static readonly int Buff1 = Animator.StringToHash(nameof(Buff1));
        public static readonly int Buff2 = Animator.StringToHash(nameof(Buff2));
        public static readonly int Buff3 = Animator.StringToHash(nameof(Buff3));

        public static readonly int Dead = Animator.StringToHash(nameof(Dead));
        public static readonly int Hit = Animator.StringToHash(nameof(Hit));
        public static readonly int Stun = Animator.StringToHash(nameof(Stun));

        public static readonly int Ground = Animator.StringToHash(nameof(Ground));
        public static readonly int Jump = Animator.StringToHash(nameof(Jump));
        public static readonly int Fall = Animator.StringToHash(nameof(Fall));

        public static readonly int Idle = Animator.StringToHash(nameof(Idle));
        public static readonly int Run = Animator.StringToHash(nameof(Run));
        public static readonly int Walk = Animator.StringToHash(nameof(Walk));

        public static readonly int ShieldAttack = Animator.StringToHash(nameof(Run));
        public static readonly int SwordAttack = Animator.StringToHash(nameof(SwordAttack));
    }

    public enum VerticalState
    {
        Ground,
        DoesTopMove,
        Fall
    }

    public enum MoveState
    {
        Idle,
        Walk,
        Run
    }

    public enum LookDirection
    {
        Rigth,
        Left
    }
}

public static class ItemsAnimatorData
{
    public static class Params
    {
        public static readonly int PickUp = Animator.StringToHash(nameof(PickUp));
    }
}