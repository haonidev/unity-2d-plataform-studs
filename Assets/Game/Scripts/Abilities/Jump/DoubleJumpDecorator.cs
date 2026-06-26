using UnityEngine;

public class DoubleJumpDecorator : JumpDecorator
{
    private int jumpsUsed;
    [SerializeField] private int maxAirJumps = 1;

    public override bool CanJump(bool baseCanJump)
    {
        if (baseCanJump)
            return true;

        if (jumpsUsed < maxAirJumps)
        {
            jumpsUsed++;
            return true;
        }

        return false;
    }

    public override void OnJumpExecuted()
    {
        // nada obrigatório aqui
    }

    public override void OnGrounded()
    {
        jumpsUsed = 0;
    }
}