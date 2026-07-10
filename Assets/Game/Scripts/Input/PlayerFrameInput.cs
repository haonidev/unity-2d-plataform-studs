using UnityEngine;

/// <summary>
/// Representa todas as entradas do jogador durante um único frame.
/// O PlayerInputReader escreve nesta estrutura e as abilities apenas fazem leitura dela.
/// </summary>
public class PlayerFrameInput
{
    /// <summary>
    /// Entrada de movimento do jogador.
    /// </summary>
    public Vector2 Move { get; set; }

    /// <summary>
    /// Indica se o botão de pulo foi pressionado neste frame.
    /// </summary>
    public bool JumpPressed { get; set; }

    /// <summary>
    /// Indica se o botão de pulo foi liberado neste frame.
    /// </summary>
    public bool JumpReleased { get; set; }

    /// <summary>
    /// Indica se o botão de dash foi pressionado neste frame.
    /// </summary>
    public bool DashPressed { get; set; }

    /// <summary>
    /// Limpa todas as ações transitórias do frame, preservando apenas entradas contínuas como Move.
    /// </summary>
    public void ResetFrameActions()
    {
        JumpPressed = false;
        JumpReleased = false;
        DashPressed = false;
    }
}