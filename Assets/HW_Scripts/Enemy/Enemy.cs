using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Patrol))]
[RequireComponent(typeof(Pursuer))]
[RequireComponent(typeof(Attaking))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Health))]
[RequireComponent(typeof(Animator))]
public class Enemy : MonoBehaviour
{
    [SerializeField] private AttackArea _attackArea;
    [SerializeField] private State _startState = State.Patrol;

    private Health _health;
    private Animator _animator;
    private Patrol _patrol;
    private Pursuer _pursuer;
    private Attaking _attaking;
    private SpriteRenderer _spriteRenderer;

    private float _lastXPosition;
    private Dictionary<State, MonoBehaviour> _states;

    private Health _targetHealth;
    public Vector2 LookDirection { get; private set; }

    private enum State
    {
        Patrol,
        Purser,
        Attaking
    }

    private void Start()
    {
        _lastXPosition = transform.position.x;

        _patrol = GetComponent<Patrol>();
        _pursuer = GetComponent<Pursuer>();
        _attaking = GetComponent<Attaking>();

        _spriteRenderer = GetComponent<SpriteRenderer>();
        _health = GetComponent<Health>();
        _animator = GetComponent<Animator>();

        _states = new Dictionary<State, MonoBehaviour>()
        {
            { State.Patrol, _patrol },
            { State.Purser, _pursuer },
            { State.Attaking, _attaking }
        };

        _attackArea.PlayerInArea += CheckArea;
        SelectAction(_startState);

        _health.Die += CheckDie;
    }

    private void Update()
    {
        CheckLookDirection();
    }

    public void SetTarget(Health target)
    {
        _targetHealth = target;
        SelectAction(State.Purser);
        _pursuer.SetTarget(_targetHealth);
    }

    private void CheckDie()
    {
        _attackArea.PlayerInArea -= CheckArea;

        foreach (var item in _states.Values)
        {
            item.enabled = false;
        }

        _animator.SetTrigger(EnemyAnimatorData.Params.Dead);
    }

    private void CheckArea(bool isTargetInArea)
    {
        if (isTargetInArea)
        {
            _attaking.SetTarget(_targetHealth);
            SelectAction(State.Attaking);
        }
        else
        {
            SelectAction(_targetHealth == null ? State.Patrol : State.Purser);
        }
    }

    private void Dead()
    {
        gameObject.SetActive(false);
    }

    private void SelectAction(State action)
    {
        foreach (State currentState in _states.Keys)
            _states[currentState].enabled = currentState == action;
    }

    private void CheckLookDirection()
    {
        float currentXPosition = transform.position.x;

        if (currentXPosition == _lastXPosition)
        {
            return;
        }
        else if (currentXPosition > _lastXPosition)
        {
            LookDirection = Vector2.right;
            _spriteRenderer.flipX = false;
        }
        else if (currentXPosition < _lastXPosition)
        {
            LookDirection = Vector2.left;
            _spriteRenderer.flipX = true;
        }

        _lastXPosition = transform.position.x;
    }
}