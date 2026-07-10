using UnityEngine;

/// <summary>
/// Base para extensões do comportamento de pulo sem alterar a lógica principal de JumpAbility.
/// </summary>
public abstract class JumpDecorator : MonoBehaviour
{
    protected JumpAbility Ability { get; private set; }

    /// <summary>
    /// Vincula o decorator à habilidade de pulo que ele estende.
    /// </summary>
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
    /// Chamado imediatamente após a execução de um salto.
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
    /// Chamado a cada frame enquanto a JumpAbility estiver ativa.
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