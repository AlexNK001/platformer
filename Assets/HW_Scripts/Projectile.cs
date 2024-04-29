using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(Animator))]
public class Projectile : MonoBehaviour
{
    [SerializeField] private float _damage = 20f;
    [SerializeField] private float _speed = 3f;

    private Vector2 _target;
    private Animator _animator;

    private void Start()
    {
        Enable();
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (transform.position.x == _target.x && transform.position.y == _target.y)
        {
            _animator.SetTrigger(EnemyAnimatorData.Projectile.Hit);
        }
        else
        {
            transform.position = Vector2.MoveTowards(
                transform.position,
                _target,
                _speed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Player target))
        {
            target.Health.TakeDamage(_damage);
            _animator.SetTrigger(EnemyAnimatorData.Projectile.Hit);
        }
    }

    public void SetTarget(Vector2 startPosition, Vector2 position)
    {
        transform.SetParent(null);
        gameObject.SetActive(true);
        transform.position = startPosition;
        _target = position;
    }

    private void Enable()
    {
        gameObject.SetActive(false);
    }
}