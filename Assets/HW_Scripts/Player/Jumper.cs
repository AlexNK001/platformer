using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Jumper : MonoBehaviour
{
    [SerializeField, Min(0f)] private float _jumpForse = 15f;
    [SerializeField, Min(0f)] private float _delayBeforeJump = 0.15f;
    [SerializeField, Min(0f)] private float _minJumpTime = 0.15f;
    [SerializeField, Min(0f)] private float _maxJumpTime = 0.35f;

    [SerializeField] private ContactFilter2D _contactFilter;
    [SerializeField] private Vector2 _size = new(0.6f, 0.6f);

    [SerializeField, Range(0, 6)] private int _digits;

    private Rigidbody2D _rigidbody;
    private RaycastHit2D[] _hits = new RaycastHit2D[1];
    private WaitForSeconds _waitingBeforeJumping;
    private WaitForSeconds _waitMinJumpTime;
    private float _currentHight;
    private Coroutine _jumpDelay;
    private bool _isJump;

    public UnityAction<PlayerAnimatorData.VerticalState> StateChanged;

    public bool IsGround { get; private set; }

    private void Start()
    {
        _waitingBeforeJumping = new WaitForSeconds(_delayBeforeJump);
        _waitMinJumpTime = new WaitForSeconds(_minJumpTime);
    }

    public void Initialization(Rigidbody2D rigidbody)
    {
        _rigidbody = rigidbody;
    }

    public void Subscribe(UserInput userInput)
    {
        userInput.Jumped += CheckJumpButtonPressed;
    }

    public void Unsubscribe(UserInput userInput)
    {
        userInput.Jumped -= CheckJumpButtonPressed;
    }

    private void FixedUpdate()
    {
        int count = Physics2D.BoxCast(transform.position, _size, 0f, Vector2.down, _contactFilter, _hits, 1.1f);

        if (count > 0)
        {
            IsGround = true;

            if (_hits[0].collider.TryGetComponent(out MovingPlatform platform))
            {
                transform.SetParent(platform.transform);
            }

            StateChanged?.Invoke(PlayerAnimatorData.VerticalState.Ground);
        }
        else
        {
            IsGround = false;

            transform.parent = null;

            float hight = MathF.Round(transform.position.y, _digits);

            if (_currentHight < hight)
            {
                StateChanged?.Invoke(PlayerAnimatorData.VerticalState.DoesTopMove);
            }
            else if (_currentHight > hight)
            {
                StateChanged?.Invoke(PlayerAnimatorData.VerticalState.Fall);
            }

            _currentHight = hight;
        }
    }

    private void CheckJumpButtonPressed(bool isJump)
    {
        _isJump = isJump;

        if (IsGround)
        {
            StateChanged?.Invoke(PlayerAnimatorData.VerticalState.DoesTopMove);
            _jumpDelay = StartCoroutine(Jump());
        }
    }

    private IEnumerator Jump()
    {
        float timeCancelJump = _maxJumpTime - _minJumpTime;

        yield return _waitingBeforeJumping;

        _rigidbody.velocity = Vector2.up * _jumpForse;

        yield return _waitMinJumpTime;

        while (timeCancelJump > 0)
        {
            timeCancelJump -= Time.deltaTime;

            if (_isJump == false)
                break;

            yield return null;
        }

        _rigidbody.velocity = Vector2.down;
        StopCoroutine(_jumpDelay);
    }
}