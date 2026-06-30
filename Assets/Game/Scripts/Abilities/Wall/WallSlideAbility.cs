using UnityEngine;

public class WallSlideAbility : Ability
{
    [SerializeField]
    private float slideSpeed = 2f;


    public override void Initialize()
    {
        base.Initialize();

        Context.State.WallSlidingChanged += OnWallSlidingChanged;
    }

    private void OnWallSlidingChanged(bool isSliding)
    {
        Debug.Log($"Evento WallSlidingChanged: {isSliding}");
    }

    /// <summary>
    /// Atalho para o estado de Wall Slide armazenado no CharacterState.
    /// </summary>
    private bool IsWallSliding
    {
        get => Context.State.IsWallSliding;
        set => Context.State.SetWallSliding(value);
    }

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

        Debug.Log($"WallSliding State = {Context.State.IsWallSliding}");
    }

    private bool CanWallSlide()
    {
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

    private void BeginWallSlide()
    {
        IsWallSliding = true;
    }

    private void EndWallSlide()
    {
        IsWallSliding = false;
    }

    private void ApplyWallSlide()
    {
        Context.Motor.ClampFallSpeed(slideSpeed);
    }

    private void OnDestroy()
    {
        if (Context != null && Context.State != null)
        {
            Context.State.WallSlidingChanged -= OnWallSlidingChanged;
        }
    }
}