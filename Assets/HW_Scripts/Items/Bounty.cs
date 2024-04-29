using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Animator))]
public class Bounty : MonoBehaviour
{
    [SerializeField] private float _timeBeforeDisappearance = 0.3f;

    private Collider2D _collider;
    private Animator _animator;

    private void OnEnable()
    {
        _collider = GetComponent<Collider2D>();
        _animator = GetComponent<Animator>();
    }

    public void PickUp()
    {
        _collider.enabled = false;
        _animator.SetTrigger(ItemsAnimatorData.Params.PickUp);
        Invoke(nameof(Disable), _timeBeforeDisappearance);
    }

    private void Disable()
    {
        gameObject.SetActive(false);
    }
}