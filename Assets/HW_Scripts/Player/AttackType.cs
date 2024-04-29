using System.Collections.Generic;
using UnityEngine;

public class AttackType : MonoBehaviour
{
    [SerializeField] private float _damage = 10f;
    [SerializeField] private float _damageDistance = 2f;
    [SerializeField] private ContactFilter2D _contactFilter = new();

    private List<RaycastHit2D> _hits = new();
    private Vector2 _damageDirection;

    public void Subscribe(UserInput userInput, AnimatorHandler animatorHandler)
    {
        userInput.DirectionChanged += Che�kDirection;
        animatorHandler.AttackCompleted += PlayerAttack;
    }

    public void Unsubscribe(UserInput userInput)
    {
        userInput.DirectionChanged -= Che�kDirection;
    }

    private void Che�kDirection(PlayerAnimatorData.LookDirection lookDirection)
    {
        _damageDirection = lookDirection == PlayerAnimatorData.LookDirection.Rigth ? Vector2.right : Vector2.left;
    }

    public void PlayerAttack()
    {
        Physics2D.Raycast(transform.position, _damageDirection, _contactFilter, _hits, _damageDistance);

        for (int i = 0; i < _hits.Count; i++)
        {
            if (_hits[i].collider.TryGetComponent(out Health enemy))
            {
                enemy.TakeDamage(_damage);
            }
        }
    }
}