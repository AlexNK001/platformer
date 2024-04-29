using UnityEngine;

public static class EnemyAnimatorData
{
    public static class Params
    {
        public static readonly int Dead = Animator.StringToHash(nameof(Dead));
        public static readonly int Attack = Animator.StringToHash(nameof(Attack));
    }

    public static class Projectile
    {
        public static readonly int Hit = Animator.StringToHash(nameof(Hit));
        public static readonly int Fly = Animator.StringToHash(nameof(Fly));
    }
}