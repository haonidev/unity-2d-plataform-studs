using UnityEngine;

public abstract class JumpDecorator : MonoBehaviour
{
    protected JumpAbility Ability { get; private set; }

    public virtual void Initialize(JumpAbility ability)
    {
        Ability = ability;
    }

    /// <summary>
    /// Permite alterar a decisão de executar um salto.
    /// </summary>
    public virtual bool CanJump(bool canJump)
    {
        return canJump;
    }

    /// <summary>
    /// Chamado imediatamente após um salto ser executado.
    /// </summary>
    public virtual void OnJumpExecuted()
    {
    }

    /// <summary>
    /// Chamado quando o personagem toca o chão.
    /// </summary>
    public virtual void OnGrounded()
    {
    }

    /// <summary>
    /// Chamado todo Update enquanto a JumpAbility está ativa.
    /// </summary>
    public virtual void Tick()
    {
    }

    /// <summary>
    /// Chamado quando o botão de salto é liberado.
    /// </summary>
    public virtual void OnJumpReleased()
    {
    }
}