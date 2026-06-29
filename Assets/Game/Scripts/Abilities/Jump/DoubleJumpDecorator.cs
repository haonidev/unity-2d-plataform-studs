using UnityEngine;

public class DoubleJumpDecorator : JumpDecorator
{
    [SerializeField]
    private int maxAirJumps = 1;

    private int jumpsUsed;

    private bool consumeAirJump;

    public override bool CanJump(bool baseCanJump)
    {
        consumeAirJump = false;

        if (baseCanJump)
        {
            return true;
        }

        if (jumpsUsed < maxAirJumps)
        {
            consumeAirJump = true;
            return true;
        }

        return false;
    }

    public override void OnJumpExecuted()
    {
        if (consumeAirJump)
        {
            jumpsUsed++;
            consumeAirJump = false;
        }
    }

    public override void OnGrounded()
    {
        jumpsUsed = 0;
        consumeAirJump = false;
    }
}