using UnityEngine;

/// <summary>
/// Controla a habilidade de dash do personagem, incluindo cooldown, duração e estado de execução.
/// </summary>
public class DashAbility : Ability
{
    [Header("Dash")]

    [SerializeField]
    private float dashDuration = 0.18f;

    [SerializeField]
    private float dashSpeed = 18f;

    private int dashDirection;

    private float dashTimer;

    [Header("Cooldown")]

    [SerializeField]
    private float dashCooldown = 0.25f;

    /// <summary>
    /// Tempo restante até que um novo dash possa ser iniciado.
    /// </summary>
    private float cooldownTimer;

    /// <summary>
    /// Indica se o dash aéreo ainda está disponível.
    /// </summary>
    private bool airDashAvailable = true;

    /// <summary>
    /// Utilizado para detectar quando o personagem acabou de tocar o chão.
    /// </summary>
    private bool wasGrounded;

    /// <summary>
    /// Gerencia o ciclo de vida do dash em cada frame de Update.
    /// </summary>
    public override void Tick()
    {
        UpdateCooldown();

        UpdateGroundReset();

        if (Context.State.IsDashing)
        {
            UpdateDash();
            return;
        }

        if (!CanStartDash())
            return;

        StartDash();
    }

    /// <summary>
    /// Aplica a velocidade do dash e remove a gravidade durante a execução.
    /// </summary>
    public override void FixedTick()
    {
        if (!Context.State.IsDashing)
            return;

        Context.Motor.RequestHorizontalVelocity(
            dashDirection * dashSpeed,
            MotorPriority.Dash);

        Context.Motor.RequestGravityScale(
            0f,
            MotorPriority.Dash);
    }

    /// <summary>
    /// Atualiza o tempo restante do cooldown.
    /// </summary>
    private void UpdateCooldown()
    {
        if (cooldownTimer <= 0f)
            return;

        cooldownTimer -= Time.deltaTime;
    }

    /// <summary>
    /// Verifica se o dash pode ser iniciado com base em input, cooldown e estado atual.
    /// </summary>
    private bool CanStartDash()
    {
        if (!Context.DashPressed)
            return false;

        if (cooldownTimer > 0f)
            return false;

        if (Context.State.IsGrounded)
            return true;

        return airDashAvailable;
    }

    /// <summary>
    /// Inicia um novo dash e altera o estado do personagem.
    /// </summary>
    private void StartDash()
    {
        dashTimer = dashDuration;

        dashDirection = Context.State.FacingDirection;

        Context.State.SetDashing(true);
        Context.Motor.LockHorizontalControl(dashDuration);

        cooldownTimer = dashCooldown;

        if (!Context.State.IsGrounded)
        {
            airDashAvailable = false;
        }
    }

    /// <summary>
    /// Atualiza a duração do dash ativo.
    /// </summary>
    private void UpdateDash()
    {
        dashTimer -= Time.deltaTime;

        if (dashTimer <= 0f)
        {
            EndDash();
        }
    }

    /// <summary>
    /// Finaliza o dash ativo.
    /// </summary>
    private void EndDash()
    {
        Context.State.SetDashing(false);
    }

    /// <summary>
    /// Restaura o dash aéreo quando o personagem toca o chão.
    /// </summary>
    private void UpdateGroundReset()
    {
        if (Context.State.IsGrounded && !wasGrounded)
        {
            airDashAvailable = true;
        }

        wasGrounded = Context.State.IsGrounded;
    }
}