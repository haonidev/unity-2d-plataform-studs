// Responsabilidade: Observar o CharacterState e atualizar o Animator.

using System.Configuration.Assemblies;
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

    private void Start()
    {
        context.State.WallSlidingChanged += OnWallSlidingChanged;
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
        context.State.DashTriggered += OnDashTriggered;
        context.State.AttackTriggered += OnAttackTriggered;
        context.State.HurtTriggered += OnHurtTriggered;

        // Sincronização inicial do personagem.
        SynchronizeAnimator();
    }

    private void OnDisable()
    {
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
        context.State.DashTriggered -= OnDashTriggered;
        context.State.AttackTriggered -= OnAttackTriggered;
        context.State.HurtTriggered -= OnHurtTriggered;
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


    // // Estados contínuos
    // context.State.RunningChanged -= OnRunningChanged;
    /// <summary>
    /// Atualiza a animação de corrida.
    /// </summary>
    private void OnRunningChanged(bool isRunning)
    {
        animator.SetBool("IsRunning", isRunning);
    }

    // context.State.GroundedChanged -= OnGroundedChanged;
    private void OnGroundedChanged(bool isGrounded)
    {
        animator.SetBool("IsGrounded", isGrounded);
    }

    // context.State.RisingChanged -= OnRisingChanged;
    private void OnRisingChanged(bool isRising)
    {
        Debug.Log($"IsRising = {isRising}");
        animator.SetBool("IsRising", isRising);
    }

    // context.State.FallingChanged -= OnFallingChanged;
    private void OnFallingChanged(bool isFalling)
    {
        Debug.Log($"IsFalling = {isFalling}");
        animator.SetBool("IsFalling", isFalling);
    }

    // context.State.WallSlidingChanged -= OnWallSlidingChanged;
    /// <summary>
    /// Atualiza a animação de Wall Slide.
    /// </summary>
    private void OnWallSlidingChanged(bool isSliding)
    {
        animator.SetBool("IsWallSliding", isSliding);
    }

    // context.State.DashingChanged -= OnDashingChanged;
    private void OnDashingChanged(bool isDashing)
    {
        animator.SetBool("IsDashing", isDashing);
    }

    // context.State.FacingDirectionChanged -= OnFacingDirectionChanged;
    /// <summary>
    /// Atualiza a direção visual do personagem.
    /// </summary>
    private void OnFacingDirectionChanged(int direction)
    {
        if (direction == 0)
            return;

        spriteRenderer.flipX = direction < 0;
    }


    // // Eventos instantâneos
    // context.State.JumpTriggered -= OnJumpTriggered;
    private void OnJumpTriggered()
    {
        animator.SetTrigger("Jump");
    }


    // context.State.DoubleJumpTriggered -= OnDoubleJumpTriggered;
    private void OnDoubleJumpTriggered()
    {
        animator.SetTrigger("DoubleJump");
    }


    // context.State.DashTriggered -= OnDashTriggered;
    private void OnDashTriggered()
    {
        animator.SetTrigger("Dash");
    }


    // context.State.AttackTriggered -= OnAttackTriggered;
    private void OnAttackTriggered()
    {
        animator.SetTrigger("Attack");
    }


    // context.State.HurtTriggered -= OnHurtTriggered;
    private void OnHurtTriggered()
    {
        animator.SetTrigger("Hurt");
    }

}