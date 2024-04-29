using UnityEngine;

public class HealingPotion : Bounty
{
    [SerializeField] private float _healPower = 20f;
    public float HealPower => _healPower;
}
