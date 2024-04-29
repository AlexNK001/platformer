using UnityEngine;

public class Pursuer : MonoBehaviour
{
    [SerializeField] private float _speed = 6f;
    [SerializeField] private Projectile _projectile;
    [SerializeField] private float _pursuerTime = 8f;

    private Health _targetHealth;
    private Enemy _enemy;

    private void Start()
    {
        _enemy = GetComponent<Enemy>();
    }

    private void Update()
    {
        Move();
    }

    private void OnEnable()
    {
        Invoke(nameof(ShootFireBool), Random.Range(0f, _pursuerTime));
    }

    private void OnDisable()
    {
        CancelInvoke(nameof(ShootFireBool));
    }

    public void SetTarget(Health target)
    {
        _targetHealth = target;
        Invoke(nameof(ShootFireBool), Random.Range(0f, _pursuerTime));
    }

    private void Move()
    {
        transform.position = Vector2.MoveTowards(
            transform.position,
            _targetHealth.transform.position,
            _speed * Time.deltaTime);
    }

    private void ShootFireBool()
    {
        _projectile.SetTarget(transform.position, _targetHealth.transform.position);
    }
}