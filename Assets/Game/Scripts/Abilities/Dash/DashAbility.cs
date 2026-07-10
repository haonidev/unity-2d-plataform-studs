using UnityEngine;

/// <summary>
/// Responsável por controlar a habilidade de Dash.
/// Nesta primeira etapa apenas controla o estado do Dash.
/// O movimento será implementado posteriormente.
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
    /// Tempo restante até que um novo Dash possa ser iniciado.
    /// </summary>
    private float cooldownTimer;

    /// <summary>
    /// Indica se o Dash aéreo ainda está disponível.
    /// </summary>
    private bool airDashAvailable = true;

    /// <summary>
    /// Utilizado para detectar quando o personagem acabou de tocar o chão.
    /// </summary>
    private bool wasGrounded;

    public override void Tick()
    {
        UpdateCooldown();

        UpdateGroundReset();

        Debug.Log("Dash Tick");
        if (Context.State.IsDashing)
        {
            UpdateDash();
            return;
        }

        if (!CanStartDash())
            return;

        StartDash();
    }

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

    private void StartDash()
    {
        dashTimer = dashDuration;

        dashDirection = Context.State.FacingDirection;

        // Segurança: caso o personagem ainda não tenha uma direção válida.
        if (dashDirection == 0)
            dashDirection = 1;

        Context.State.SetDashing(true);
        Context.Motor.LockHorizontalControl(dashDuration);

        cooldownTimer = dashCooldown;

        if (!Context.State.IsGrounded)
        {
            airDashAvailable = false;
        }
    }

    private void UpdateDash()
    {
        dashTimer -= Time.deltaTime;

        if (dashTimer <= 0f)
        {
            EndDash();
        }
    }

    private void EndDash()
    {
        Context.State.SetDashing(false);
    }

    /// <summary>
    /// Restaura o Dash quando o personagem toca o chão.
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