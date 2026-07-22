using UnityEngine;

/// <summary>
/// Habilita o wall slide quando o personagem está encostado em uma parede e caindo.
/// </summary>
public class WallSlideAbility : Ability
{
    [SerializeField]
    private float slideSpeed = 2f;

    /// <summary>
    /// Registra o callback de alteração do estado de wall slide.
    /// </summary>
    public override void Initialize()
    {
        base.Initialize();

        Context.State.WallSlidingChanged += OnWallSlidingChanged;
    }

    /// <summary>
    /// Mantém o callback registrado enquanto a habilidade existir.
    /// TODO: este método pode ser substituído por um pattern de observabilidade mais explícito no futuro.
    /// </summary>
    private void OnWallSlidingChanged(bool isSliding)
    {
    }

    /// <summary>
    /// Atalho para o estado de wall slide armazenado no CharacterState.
    /// </summary>
    private bool IsWallSliding
    {
        get => Context.State.IsWallSliding;
        set => Context.State.SetWallSliding(value);
    }

    /// <summary>
    /// Atualiza o estado de wall slide a cada frame.
    /// </summary>
    public override void Tick()
    {
        bool shouldSlide = CanWallSlide();

        if (shouldSlide && !IsWallSliding)
        {
            BeginWallSlide();
        }
        else if (!shouldSlide && IsWallSliding)
        {
            EndWallSlide();
        }

        if (IsWallSliding)
        {
            ApplyWallSlide();
        }
    }

    /// <summary>
    /// Verifica se as condições para wall slide estão presentes.
    /// </summary>
    private bool CanWallSlide()
    {
        if (!Context.State.HasControl)
            return false;

        if (Context.IsGrounded)
            return false;

        if (!Context.IsTouchingWall)
            return false;

        if (!Context.IsFalling)
            return false;

        if (Context.MoveInput.x == 0f)
            return false;

        return Mathf.Sign(Context.MoveInput.x) == Context.WallDirection;
    }

    /// <summary>
    /// Inicia o estado de wall slide.
    /// </summary>
    private void BeginWallSlide()
    {
        IsWallSliding = true;
    }

    /// <summary>
    /// Finaliza o estado de wall slide.
    /// </summary>
    private void EndWallSlide()
    {
        IsWallSliding = false;
    }

    /// <summary>
    /// Limita a velocidade de queda durante o wall slide.
    /// </summary>
    private void ApplyWallSlide()
    {
        Context.Motor.ClampFallSpeed(slideSpeed);
    }

    /// <summary>
    /// Remove o callback de mudança de estado ao destruir a habilidade.
    /// </summary>
    private void OnDestroy()
    {
        if (Context != null && Context.State != null)
        {
            Context.State.WallSlidingChanged -= OnWallSlidingChanged;
        }
    }
}