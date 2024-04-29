using System;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private float _health = 100f;
    [SerializeField] private TextChangeHealth _prefab;
    [SerializeField] private Canvas _parent;

    private List<TextChangeHealth> _cells;
    private float _maxHealth;

    public Action<float> HealthChanged;
    public Action Die;

    public bool IsAlive => _health > 0;

    private void Start()
    {
        _maxHealth = _health;

        _cells = new List<TextChangeHealth>();

        for (int i = 0; i < 5; i++)
        {
            TextChangeHealth cell = Instantiate(_prefab, _parent.transform, false);
            _cells.Add(cell);
        }
    }

    private TextChangeHealth FindFreeCell()
    {
        foreach (TextChangeHealth cell in _cells)
        {
            if (cell.IsWork == false)
            {
                return cell;
            }
        }

        TextChangeHealth newCell = Instantiate(_prefab, transform);
        _cells.Add(newCell);
        return newCell;
    }

    public void TakeDamage(float damage)
    {
        _health -= damage;
        HealthChanged?.Invoke(_health / _maxHealth);

        FindFreeCell().ShowDamage(damage);

        if (_health <= 0)
        {
            Die.Invoke();
        }
    }

    public void Healing(float heal)
    {
        _health += heal;
        HealthChanged.Invoke(_health / _maxHealth);

        FindFreeCell().ShowHeal(heal);

        if (_health > _maxHealth)
        {
            _health = _maxHealth;
        }
    }
}
