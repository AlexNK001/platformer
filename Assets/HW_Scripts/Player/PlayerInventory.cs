using System;
using UnityEngine;
using System.Collections.Generic;

public class PlayerInventory : MonoBehaviour
{
    private int _coinCount = 0;
    private Queue<HealingPotion> _healingPotions = new();

    private Health _battleHandler;

    public Action<int> CoinCountChanged;
    public Action<int> HealingPotionCountChanged;

    public bool TryHeal => _healingPotions.Count > 0;

    private void Start()
    {
        CoinCountChanged.Invoke(_coinCount);
        HealingPotionCountChanged.Invoke(_healingPotions.Count);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Bounty item))
        {
            item.PickUp();

            switch (item)
            {
                case Coin coin:
                    CoinCountChanged.Invoke(++_coinCount);
                    break;

                case HealingPotion healingPotion:
                    _healingPotions.Enqueue(healingPotion);
                    HealingPotionCountChanged.Invoke(_healingPotions.Count);
                    break;
            }
        }
    }

    public void Initialization(Health battleHandler)
    {
        _battleHandler = battleHandler;
    }

    public void Subscribe(AnimatorHandler animatorHandler)
    {
        animatorHandler.HealingCompleted += Healing;
    }

    public void Unsubscribe(AnimatorHandler animatorHandler)
    {
        animatorHandler.HealingCompleted -= Healing;
    }

    private void Healing()
    {
        _battleHandler.Healing(_healingPotions.Dequeue().HealPower);
        HealingPotionCountChanged.Invoke(_healingPotions.Count);
    }
}
