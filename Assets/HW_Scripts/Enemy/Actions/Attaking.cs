using System.Collections;
using UnityEngine;

public class Attaking : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private float _damage = 10f;
    [SerializeField] private float _timeBeforeAttacks = 1f;

    private Health _targetHealth;

    private Coroutine _repiatAttacks;
    private WaitForSeconds _waitingSecondAttack;

    private void Start()
    {
        _waitingSecondAttack = new WaitForSeconds(_timeBeforeAttacks);
    }

    private void OnEnable()
    {
        _repiatAttacks = StartCoroutine(StartFight());
    }

    private void OnDisable()
    {
        if (_repiatAttacks != null)
        {
            StopCoroutine(_repiatAttacks);
        }
    }

    public void SetTarget(Health target)
    {
        _targetHealth = target;
        _repiatAttacks = StartCoroutine(StartFight());
    }

    private IEnumerator StartFight()
    {
        while (_targetHealth.IsAlive)
        {
            _animator.SetTrigger(EnemyAnimatorData.Params.Attack);
            _targetHealth.TakeDamage(_damage);

            yield return _waitingSecondAttack;
        }

        StopCoroutine(_repiatAttacks);
    }
}