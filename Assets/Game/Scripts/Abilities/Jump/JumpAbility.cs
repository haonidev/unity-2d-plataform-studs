using UnityEngine;

/// <summary>
/// Implementa a lógica principal de pulo com coyote time, jump buffer e extensão via decorators.
/// </summary>
public class JumpAbility : Ability
{
    [Header("Jump Settings")]
    [SerializeField] private float jumpForce = 12f;

    [Header("Timers")]
    [SerializeField] private float coyoteTime = 0.12f;
    [SerializeField] private float jumpBufferTime = 0.12f;

    public CharacterContext CharacterContext => Context;
    private JumpDecorator[] decorators;

    private bool wasGrounded;

    private float coyoteCounter;
    private float bufferCounter;

    /// <summary>
    /// Inicializa os decorators de pulo associados a esta habilidade.
    /// </summary>
    public override void Initialize()
    {
        base.Initialize();

        decorators = GetComponents<JumpDecorator>();

        foreach (var d in decorators)
            d.Initialize(this);
    }

    /// <summary>
    /// Atualiza coyote time, buffer de salto e executa a lógica de jump no Update.
    /// </summary>
    public override void Tick()
    {
        UpdateCoyoteTime();
        UpdateJumpBuffer();

        TryConsumeJump();

        if (Context.JumpReleased)
        {
            foreach (var decorator in decorators)
            {
                decorator.OnJumpReleased();
            }
        }

        foreach (var decorator in decorators)
        {
            decorator.Tick();
        }
    }

    /// <summary>
    /// Atualiza os contadores de coyote time e notifica decorators quando o personagem pousa.
    /// </summary>
    private void UpdateCoyoteTime()
    {
        if (Context.IsGrounded)
        {
            coyoteCounter = coyoteTime;

            if (!wasGrounded)
            {
                foreach (var d in decorators)
                    d.OnGrounded();
            }

            wasGrounded = true;
        }
        else
        {
            wasGrounded = false;
            coyoteCounter -= Time.deltaTime;
        }
    }

    /// <summary>
    /// Atualiza o buffer de salto com base na entrada do frame.
    /// </summary>
    private void UpdateJumpBuffer()
    {
        if (Context.JumpPressed)
            bufferCounter = jumpBufferTime;
        else
            bufferCounter -= Time.deltaTime;
    }

    /// <summary>
    /// Tenta executar o salto se houver buffer e permissões válidas.
    /// </summary>
    private void TryConsumeJump()
    {
        if (bufferCounter <= 0) return;

        bool canJump = coyoteCounter > 0;

        foreach (var d in decorators)
            canJump = d.CanJump(canJump);

        if (!canJump) return;

        ExecuteJump();
    }

    /// <summary>
    /// Aplica a força do salto e notifica os decorators envolvidos.
    /// </summary>
    private void ExecuteJump()
    {
        Context.Motor.SetVerticalVelocity(jumpForce);

        Context.State.TriggerJump();

        foreach (var decorator in decorators)
        {
            decorator.OnJumpExecuted();
        }

        bufferCounter = 0;
        coyoteCounter = 0;
    }
}