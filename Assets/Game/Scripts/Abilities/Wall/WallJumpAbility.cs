using UnityEngine;

/// <summary>
/// Executa saltos normais ou neutros a partir do Wall Slide, com buffer e wall coyote time.
/// </summary>
public class WallJumpAbility : Ability
{
    [Header("Wall Jump")]
    [SerializeField] private float jumpForce = 12f;
    [SerializeField] private float horizontalForce = 8f;
    [SerializeField] private float neutralJumpForce = 14f;
    [SerializeField] private float neutralHorizontalForce = 2f;

    [Header("Tolerâncias")]
    [SerializeField, Min(0f)] private float wallCoyoteTime = 0.1f;
    [SerializeField, Min(0f)] private float jumpBufferTime = 0.1f;

    [Header("Controle")]
    [SerializeField, Min(0f)] private float controlLockDuration = 0.05f;
    [SerializeField, Range(0f, 1f)] private float horizontalInputThreshold = 0.1f;

    private float wallCoyoteCounter;
    private float jumpBufferCounter;
    private int lastWallDirection;

    /// <summary>
    /// Mantém as janelas de tolerância e tenta consumir o comando de pulo no Update.
    /// </summary>
    public override void Tick()
    {
        UpdateLastWallAndCoyote();
        UpdateJumpBuffer();

        bool canWallJump = CanWallJump();
        Context.State.SetWallJumpAvailable(canWallJump);

        if (Context.FrameInput.JumpActionConsumed)
        {
            jumpBufferCounter = 0f;
            return;
        }

        if (!canWallJump || jumpBufferCounter <= 0f || !Context.State.HasControl)
            return;

        ExecuteWallJump();
    }

    /// <summary>
    /// Guarda a direção da parede enquanto o personagem está em Wall Slide e mantém o coyote.
    /// </summary>
    private void UpdateLastWallAndCoyote()
    {
        if (Context.IsGrounded)
        {
            lastWallDirection = 0;
            wallCoyoteCounter = 0f;
            return;
        }

        if (Context.IsTouchingWall && Context.WallDirection != 0)
            lastWallDirection = Context.WallDirection;

        if (Context.State.IsWallSliding && Context.IsTouchingWall && Context.WallDirection != 0)
        {
            wallCoyoteCounter = wallCoyoteTime;
            return;
        }

        wallCoyoteCounter = Mathf.Max(0f, wallCoyoteCounter - Time.deltaTime);
    }

    /// <summary>
    /// Registra o pulo antes da condição de parede tornar-se válida.
    /// </summary>
    private void UpdateJumpBuffer()
    {
        if (Context.FrameInput.JumpActionConsumed)
        {
            jumpBufferCounter = 0f;
            return;
        }

        if (Context.JumpPressed)
            jumpBufferCounter = jumpBufferTime;
        else
            jumpBufferCounter = Mathf.Max(0f, jumpBufferCounter - Time.deltaTime);
    }

    private bool CanWallJump()
    {
        return !Context.IsGrounded
            && lastWallDirection != 0
            && ((Context.State.IsWallSliding && Context.IsTouchingWall)
                || wallCoyoteCounter > 0f);
    }

    /// <summary>
    /// Escolhe salto normal ao receber direção para longe da parede; nos demais casos, neutro.
    /// </summary>
    private void ExecuteWallJump()
    {
        if (!Context.TryConsumeJumpAction())
            return;

        int jumpDirection = -lastWallDirection;
        float horizontalInput = Context.MoveInput.x;
        bool jumpingAway = Mathf.Abs(horizontalInput) >= horizontalInputThreshold
            && Mathf.Sign(horizontalInput) == jumpDirection;

        if (jumpingAway)
        {
            Context.Motor.RequestHorizontalVelocity(
                jumpDirection * horizontalForce,
                MotorPriority.WallJump);
            Context.Motor.SetVerticalVelocity(jumpForce);
            Context.Motor.LockHorizontalControl(controlLockDuration);
        }
        else
        {
            Context.Motor.RequestHorizontalVelocity(
                jumpDirection * neutralHorizontalForce,
                MotorPriority.WallJump);
            Context.Motor.SetVerticalVelocity(neutralJumpForce);
        }

        wallCoyoteCounter = 0f;
        jumpBufferCounter = 0f;
        Context.State.SetWallJumpAvailable(false);
        Context.State.SetWallSliding(false);
        Context.State.SetRising(true);
        Context.State.SetFalling(false);
        Context.State.TriggerJump();
    }
}
