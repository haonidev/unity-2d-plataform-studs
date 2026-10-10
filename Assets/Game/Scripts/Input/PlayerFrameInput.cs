using UnityEngine;

/// <summary>
/// Representa todas as entradas do jogador durante um único frame.
/// O PlayerInputReader escreve nesta estrutura e as abilities apenas fazem leitura dela.
/// </summary>
public class PlayerFrameInput
{
    private bool jumpPressed;

    /// <summary>
    /// Entrada de movimento do jogador.
    /// </summary>
    public Vector2 Move { get; set; }

    /// <summary>
    /// Indica se o botão de pulo foi pressionado neste frame.
    /// </summary>
    public bool JumpPressed
    {
        get => jumpPressed;
        set
        {
            jumpPressed = value;

            if (value)
                JumpActionConsumed = false;
        }
    }

    /// <summary>
    /// Indica que uma habilidade já consumiu a ação de pulo mais recente.
    /// Permanece ativa até uma nova pressão, evitando buffers residuais entre habilidades.
    /// Evita que habilidades de pulo concorrentes executem pelo mesmo comando.
    /// </summary>
    public bool JumpActionConsumed { get; private set; }

    /// <summary>
    /// Indica se o botão de pulo foi liberado neste frame.
    /// </summary>
    public bool JumpReleased { get; set; }

    /// <summary>
    /// Indica se o botão de dash foi pressionado neste frame.
    /// </summary>
    public bool DashPressed { get; set; }

    /// <summary>
    /// Reserva a ação de pulo atual para a habilidade que conseguiu executá-la.
    /// </summary>
    public bool TryConsumeJumpAction()
    {
        if (JumpActionConsumed)
            return false;

        JumpActionConsumed = true;
        return true;
    }

    /// <summary>
    /// Limpa os sinais transitórios do frame. A reserva do último comando de pulo só é liberada por uma nova pressão.
    /// </summary>
    public void ResetFrameActions()
    {
        JumpPressed = false;
        JumpReleased = false;
        DashPressed = false;
    }
}
