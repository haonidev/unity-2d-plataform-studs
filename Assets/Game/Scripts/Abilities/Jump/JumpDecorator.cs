using UnityEngine;

public abstract class JumpDecorator : MonoBehaviour
{
    protected JumpAbility ability;

    public virtual void Initialize(JumpAbility ability)
    {
        this.ability = ability;
    }

    public virtual bool CanJump(bool baseCanJump)
    {
        return baseCanJump;
    }

    public virtual void OnJumpExecuted() { }

    public virtual void OnGrounded() { }
}