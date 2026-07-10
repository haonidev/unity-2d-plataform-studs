using UnityEngine;

/// <summary>
/// Adiciona um salto extra no ar sem alterar a lógica base de JumpAbility.
/// </summary>
public class DoubleJumpDecorator : JumpDecorator
{
    [SerializeField]
    private int maxAirJumps = 1;

    private int jumpsUsed;

    private bool consumeAirJump;

    /// <summary>
    /// Permite a execução de um double jump quando o salto base não é válido.
    /// </summary>
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

    /// <summary>
    /// Registra o uso do salto extra após sua execução.
    /// </summary>
    public override void OnJumpExecuted()
    {
        if (consumeAirJump)
        {
            jumpsUsed++;
            consumeAirJump = false;
        }
    }

    /// <summary>
    /// Reinicia o contador de saltos extras ao tocar o chão.
    /// </summary>
    public override void OnGrounded()
    {
        jumpsUsed = 0;
        consumeAirJump = false;
    }
}