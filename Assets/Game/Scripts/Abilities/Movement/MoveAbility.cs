using UnityEngine;

public class MoveAbility : Ability
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 8f;
    [SerializeField] private float acceleration = 60f;
    [SerializeField] private float deceleration = 80f;
    [SerializeField] private float airControlMultiplier = 0.6f;

    private float currentSpeed;

    public override void FixedTick()
    {
        ApplyMovement();
    }

    private void ApplyMovement()
    {
        float inputX = Context.MoveInput.x;

        float targetSpeed = inputX * moveSpeed;

        float accelRate;

        if (Mathf.Abs(targetSpeed) > 0.01f)
        {
            accelRate = acceleration;
        }
        else
        {
            accelRate = deceleration;
        }

        if (!Context.IsGrounded)
        {
            accelRate *= airControlMultiplier;
        }

        currentSpeed = Mathf.MoveTowards(
            currentSpeed,
            targetSpeed,
            accelRate * Time.fixedDeltaTime
        );

        Context.Motor.SetHorizontalVelocity(currentSpeed);
        Context.State.SetRunning(Mathf.Abs(currentSpeed) > 0.01f);

        if (inputX > 0f)
        {
            Context.State.SetFacingDirection(1);
        }
        else if (inputX < 0f)
        {
            Context.State.SetFacingDirection(-1);
        }
    }
}