// Responsabilidade: Observar o CharacterState e atualizar o Animator.

using UnityEngine;

[RequireComponent(typeof(CharacterContext))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
public class CharacterAnimator : MonoBehaviour
{
    private CharacterContext context;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        CacheComponents();
    }

    private void CacheComponents()
    {
        context = GetComponent<CharacterContext>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {

        if (context == null)
            return;

        // Estados contínuos
        context.State.RunningChanged += OnRunningChanged;
        context.State.GroundedChanged += OnGroundedChanged;
        context.State.RisingChanged += OnRisingChanged;
        context.State.FallingChanged += OnFallingChanged;
        context.State.WallSlidingChanged += OnWallSlidingChanged;
        context.State.DashingChanged += OnDashingChanged;
        context.State.FacingDirectionChanged += OnFacingDirectionChanged;

        // Eventos instantâneos
        context.State.JumpTriggered += OnJumpTriggered;
        context.State.DoubleJumpTriggered += OnDoubleJumpTriggered;
        context.State.AttackTriggered += OnAttackTriggered;
        context.State.HurtTriggered += OnHurtTriggered;
        context.State.DeathTriggered += OnDeathTriggered;

        // Sincronização inicial do personagem.
        SynchronizeAnimator();
    }

    private void OnDisable()
    {
        if (context == null)
            return;

        // Estados contínuos
        context.State.RunningChanged -= OnRunningChanged;
        context.State.GroundedChanged -= OnGroundedChanged;
        context.State.RisingChanged -= OnRisingChanged;
        context.State.FallingChanged -= OnFallingChanged;
        context.State.WallSlidingChanged -= OnWallSlidingChanged;
        context.State.DashingChanged -= OnDashingChanged;
        context.State.FacingDirectionChanged -= OnFacingDirectionChanged;

        // Eventos instantâneos
        context.State.JumpTriggered -= OnJumpTriggered;
        context.State.DoubleJumpTriggered -= OnDoubleJumpTriggered;
        context.State.AttackTriggered -= OnAttackTriggered;
        context.State.HurtTriggered -= OnHurtTriggered;
        context.State.DeathTriggered -= OnDeathTriggered;
    }

    private void SynchronizeAnimator()
    {
        OnRunningChanged(context.State.IsRunning);

        OnGroundedChanged(context.State.IsGrounded);

        OnRisingChanged(context.State.IsRising);

        OnFallingChanged(context.State.IsFalling);

        OnWallSlidingChanged(context.State.IsWallSliding);

        OnDashingChanged(context.State.IsDashing);

        OnFacingDirectionChanged(context.State.FacingDirection);
    }

    /// <summary>
    /// Atualiza a animação de corrida.
    /// </summary>
    private void OnRunningChanged(bool isRunning)
    {
        animator.SetBool("IsRunning", isRunning);
    }

    private void OnGroundedChanged(bool isGrounded)
    {
        animator.SetBool("IsGrounded", isGrounded);
    }

    private void OnRisingChanged(bool isRising)
    {
        animator.SetBool("IsRising", isRising);
    }

    private void OnFallingChanged(bool isFalling)
    {
        animator.SetBool("IsFalling", isFalling);
    }

    /// <summary>
    /// Atualiza a animação de Wall Slide.
    /// </summary>
    private void OnWallSlidingChanged(bool isSliding)
    {
        animator.SetBool("IsWallSliding", isSliding);
    }

    private void OnDashingChanged(bool isDashing)
    {
        animator.SetBool("IsDashing", isDashing);
    }

    /// <summary>
    /// </summary>
    private void OnFacingDirectionChanged(int direction)
    {
        if (direction == 0)
            return;

        spriteRenderer.flipX = direction < 0;
    }


    // // Eventos instantâneos
    private void OnJumpTriggered()
    {
        animator.SetTrigger("Jump");
    }


    private void OnDoubleJumpTriggered()
    {
        animator.SetTrigger("DoubleJump");
    }


    private void OnAttackTriggered()
    {
        animator.SetTrigger("Attack");
    }


    private void OnHurtTriggered()
    {
        animator.SetTrigger("Hurt");
    }

    private void OnDeathTriggered()
    {
        Debug.Log("CharacterAnimator recebeu DeathTriggered");
        animator.SetTrigger("Death");
    }

    /// <summary>
    /// Chamado pelo Animation Event ao término da animação de morte.
    /// </summary>
    public void OnDeathAnimationFinished()
    {
        context.State.FinishDeathAnimation();
    }

}