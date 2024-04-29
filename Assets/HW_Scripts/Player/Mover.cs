using UnityEngine;

public class Mover : MonoBehaviour
{
    [SerializeField, Min(0f)] private float _walkSpeed = 5;
    [SerializeField, Min(0f)] private float _runSpeed = 8;

    private Jumper _jumper;

    private Vector2 _direction;

    public void Initialization(Jumper jumper)
    {
        _jumper = jumper;
    }

    public void Subscribe(UserInput userInput)
    {
        userInput.DirectionChanged += CheckDirection;
        userInput.SpeedChanged += FindNeedRun;
    }

    public void Unsubscribe(UserInput userInput)
    {
        userInput.DirectionChanged -= CheckDirection;
        userInput.SpeedChanged -= FindNeedRun;
    }

    private void FindNeedRun(PlayerAnimatorData.MoveState moveState)
    {
        float speed;

        switch (moveState)
        {
            case PlayerAnimatorData.MoveState.Walk:
                speed = _walkSpeed;
                break;

            case PlayerAnimatorData.MoveState.Run:
                speed = _jumper.IsGround ? _runSpeed : _walkSpeed;
                break;

            default:
            case PlayerAnimatorData.MoveState.Idle:
                return;
        }

        transform.Translate(_direction * Time.deltaTime * speed);
    }

    private void CheckDirection(PlayerAnimatorData.LookDirection lookDirection)
    {
        _direction = lookDirection == PlayerAnimatorData.LookDirection.Rigth ? Vector2.right : Vector2.left;
    }
}