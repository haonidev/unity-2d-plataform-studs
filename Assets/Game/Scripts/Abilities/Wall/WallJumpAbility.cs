using UnityEngine;

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

    private bool CanWallJump()
    {
        return Context.State.IsWallSliding;
    }

    private void ExecuteWallJump()
    {
        int jumpDirection = -Context.WallDirection;

        Vector3 position = transform.position;

        position.x += jumpDirection * wallDetachDistance;

        transform.position = position;

        // Impede que o MoveAbility assuma o controle imediatamente.
        Context.Motor.LockHorizontalControl(controlLockDuration);

        // Aplica o impulso horizontal.
        Context.Motor.SetHorizontalVelocity(horizontalForce * jumpDirection);

        // Aplica o impulso vertical.
        Context.Motor.SetVerticalVelocity(jumpForce);

        // Sai imediatamente do Wall Slide.
        Context.State.SetWallSliding(false);

        Context.State.SetRising(true);
        Context.State.SetFalling(false);

        // Dispara a animação de salto.
        Context.State.TriggerJump();
    }
}