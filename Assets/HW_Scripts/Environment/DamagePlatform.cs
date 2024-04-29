using UnityEngine;
using System.Collections;

public class DamagePlatform : MonoBehaviour
{
    [SerializeField] private float _reloadTime = 0.5f;
    [SerializeField] private float _damage = 10f;

    private Health _target;
    private Coroutine _regularDamage;
    private WaitForSeconds _wait;

    private void Start()
    {
        _wait = new WaitForSeconds(_reloadTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.TryGetComponent(out Health player))
        {
            _target = player;
            _regularDamage = StartCoroutine(DealRegularDamage());
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.transform.TryGetComponent(out Health player) && player == _target)
        {
            _target = null;
            StopCoroutine(_regularDamage);
        }
    }

    private IEnumerator DealRegularDamage()
    {
        while (_target != null)
        {
            _target.TakeDamage(_damage);
            yield return _wait;
        }
    }
}