using System.Collections.Generic;
using UnityEngine;

public class Patrol : MonoBehaviour
{
    [SerializeField] private float _speed = 4f;
    [SerializeField] private Transform _rightPoint;
    [SerializeField] private Transform _leftPoint;
    [SerializeField] private float _searchDistanse = 4f;
    [SerializeField] private ContactFilter2D _contactFilter;

    private Enemy _enemy;

    private List<RaycastHit2D> _raycastResult = new();
    private Transform _currentPoint;
    private Transform[] _points;
    private int _pointIndex = 0;

    private void Start()
    {
        _enemy = GetComponent<Enemy>();

        _points = new Transform[2]
        {
            _rightPoint,
            _leftPoint
        };

        _rightPoint.SetParent(null);
        _leftPoint.SetParent(null);

        _currentPoint = GetNextPoints();
    }

    private void Update()
    {
        Move();

        if (transform.position.x == _currentPoint.position.x)
            _currentPoint = GetNextPoints();

        FindTarget();
    }

    private void FindTarget()
    {
        Physics2D.BoxCast(
                    transform.position,
                    Vector2.one, 0f,
                    _enemy.LookDirection,
                    _contactFilter,
                    _raycastResult,
                    _searchDistanse);

        for (int i = 0; i < _raycastResult.Count; i++)
        {
            if (_raycastResult[i].collider.TryGetComponent(out Health target))
            {
                _enemy.SetTarget(target);
            }
        }
    }

    private void Move()
    {
        transform.position = Vector2.MoveTowards(
                    transform.position,
                    _currentPoint.position,
                    _speed * Time.deltaTime);
    }

    private Transform GetNextPoints()
    {
        _pointIndex++;
        _pointIndex %= _points.Length;

        return _points[_pointIndex];
    }
}