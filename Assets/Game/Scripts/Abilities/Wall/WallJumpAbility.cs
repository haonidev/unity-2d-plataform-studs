using UnityEngine;

/// <summary>
/// Executa o wall jump quando o personagem está em wall slide e pressiona o botão de salto.
/// </summary>
public class WallJumpAbility : Ability
{
    [Header("Wall Jump")]

    [SerializeField]
    private float jumpForce = 12f;

    [SerializeField]
    private float horizontalForce = 8f;

    [SerializeField]
    private float controlLockDuration = 0.15f;

    [SerializeField]
    private float wallDetachDistance = 0.08f;

    /// <summary>
    /// Verifica se o wall jump pode ser executado neste frame.
    /// </summary>
    public override void Tick()
    {
        bool canWallJump = CanWallJump();
        bool jumpPressed = Context.JumpPressed;

        if (!canWallJump)
            return;

        if (!jumpPressed)
            return;

        ExecuteWallJump();
    }

    /// <summary>
    /// Verifica se o personagem está em estado de wall slide.
    /// </summary>
    private bool CanWallJump()
    {
        return Context.State.IsWallSliding;
    }

    /// <summary>
    /// Aplica o impulso do wall jump e encerra o estado de wall slide.
    /// </summary>
    private void ExecuteWallJump()
    {
        int jumpDirection = -Context.WallDirection;

        Vector3 position = transform.position;

        position.x += jumpDirection * wallDetachDistance;

        transform.position = position;

        Context.Motor.LockHorizontalControl(controlLockDuration);

        Context.Motor.RequestHorizontalVelocity(horizontalForce * jumpDirection, MotorPriority.WallJump);

        Context.Motor.SetVerticalVelocity(jumpForce);

        Context.State.SetWallSliding(false);

        Context.State.SetRising(true);
        Context.State.SetFalling(false);

        Context.State.TriggerJump();
    }
}