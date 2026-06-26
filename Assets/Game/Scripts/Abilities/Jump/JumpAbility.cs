using UnityEngine;

public class JumpAbility : Ability
{
    [Header("Jump Settings")]
    [SerializeField] private float jumpForce = 12f;

    [Header("Timers")]
    [SerializeField] private float coyoteTime = 0.12f;
    [SerializeField] private float jumpBufferTime = 0.12f;
    [SerializeField] private float lowJumpMultiplier = 2.5f;

    private JumpDecorator[] decorators;

    private bool wasGrounded;

    private float coyoteCounter;
    private float bufferCounter;

    public override void Initialize()
    {
        base.Initialize();

        decorators = GetComponents<JumpDecorator>();

        foreach (var d in decorators)
            d.Initialize(this);
    }

    public override void Tick()
    {
        UpdateCoyoteTime();
        UpdateJumpBuffer();

        TryConsumeJump();

        if (Context.ConsumeJumpReleased())
        {
            ApplyVariableJump();
        }
    }

    private void ApplyVariableJump()
    {
        if (Context.ConsumeJumpReleased())
        {
            if (Context.Motor.GetVerticalVelocity() > 0)
            {
                Context.Motor.ReduceVerticalVelocity(lowJumpMultiplier);
            }
        }
    }

    // private void UpdateCoyoteTime()
    // {
    //     if (Context.IsGrounded)
    //         coyoteCounter = coyoteTime;
    //     else
    //         coyoteCounter -= Time.deltaTime;
    // }

    private void UpdateCoyoteTime()
    {
        if (Context.IsGrounded)
        {
            coyoteCounter = coyoteTime;

            if (!wasGrounded)
            {
                foreach (var d in decorators)
                    d.OnGrounded();
            }

            wasGrounded = true;
        }
        else
        {
            wasGrounded = false;
            coyoteCounter -= Time.deltaTime;
        }
    }

    private void UpdateJumpBuffer()
    {
        if (Context.ConsumeJumpPressed())
            bufferCounter = jumpBufferTime;
        else
            bufferCounter -= Time.deltaTime;
    }


    private void TryConsumeJump()
    {
        if (bufferCounter <= 0) return;

        bool canJump = coyoteCounter > 0;

        foreach (var d in decorators)
            canJump = d.CanJump(canJump);

        if (!canJump) return;

        ExecuteJump();

        foreach (var d in decorators)
            d.OnJumpExecuted();
    }

    private void ExecuteJump()
    {
        Context.Motor.SetVerticalVelocity(jumpForce);

        bufferCounter = 0;
        coyoteCounter = 0;
    }
}