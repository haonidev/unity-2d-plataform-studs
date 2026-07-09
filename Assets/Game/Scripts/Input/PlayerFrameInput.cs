using UnityEngine;

/// <summary>
/// Representa todas as entradas do jogador durante um único frame.
///
/// O PlayerInputReader escreve nesta estrutura.
/// As Abilities apenas realizam leitura.
///
/// Ao final de cada Update, o AbilityController limpa os
/// eventos transitórios através de ResetFrameActions().
/// </summary>
public class PlayerFrameInput
{
    /// <summary>
    /// Entrada de movimento.
    /// </summary>
    public Vector2 Move { get; set; }

    /// <summary>
    /// Botão de pulo pressionado neste frame.
    /// </summary>
    public bool JumpPressed { get; set; }

    /// <summary>
    /// Botão de pulo liberado neste frame.
    /// </summary>
    public bool JumpReleased { get; set; }

    /// <summary>
    /// Limpa todas as ações transitórias.
    /// Não limpa entradas contínuas como Move.
    /// </summary>
    public void ResetFrameActions()
    {
        JumpPressed = false;
        JumpReleased = false;
    }
}