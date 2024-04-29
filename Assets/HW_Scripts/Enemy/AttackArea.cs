using System;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class AttackArea : MonoBehaviour
{
    [SerializeField] private Enemy _enemyActionsHandler;

    private Collider2D _collider;
    private Player _target;

    public Action<bool> PlayerInArea;

    private void Start()
    {
        _collider = GetComponent<Collider2D>();
        _collider.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out Player target))
        {
            _target = target;
            _enemyActionsHandler.SetTarget(_target.GetComponent<Health>());
            PlayerInArea?.Invoke(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Player target) && _target == target)
        {
            PlayerInArea?.Invoke(false);
        }
    }
}
