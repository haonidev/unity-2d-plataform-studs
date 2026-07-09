using System;
using UnityEngine;

/// <summary>
/// Representa o estado atual do personagem.
/// É a única fonte de verdade para qualquer sistema que precise
/// observar o Player (Animator, Áudio, VFX, etc.).
/// </summary>
public class CharacterState : MonoBehaviour
{
    //==========================================================
    // ESTADOS CONTÍNUOS
    //==========================================================

    public bool IsGrounded { get; private set; }

    public bool IsRunning { get; private set; }

    public bool IsRising { get; private set; }

    public bool IsFalling { get; private set; }

    public bool IsWallSliding { get; private set; }

    public bool IsDashing { get; private set; }

    /// <summary>
    /// Indica se o jogador pode controlar o movimento horizontal.
    /// </summary>
    public bool HasMovementControl { get; private set; } = true;

    /// <summary>
    /// -1 = esquerda
    ///  0 = mantém direção atual
    ///  1 = direita
    /// </summary>
    public int FacingDirection { get; private set; }

    //==========================================================
    // EVENTOS DE ESTADOS CONTÍNUOS
    //==========================================================
    public event Action<bool> RunningChanged;
    public event Action<bool> GroundedChanged;
    public event Action<bool> RisingChanged;
    public event Action<bool> FallingChanged;
    public event Action<bool> WallSlidingChanged;
    public event Action<bool> DashingChanged;

    /// <summary>
    /// Disparado quando o controle horizontal é habilitado ou desabilitado.
    /// </summary>
    public event Action<bool> CanMoveHorizontallyChanged;
    public event Action<int> FacingDirectionChanged;



    //==========================================================
    // EVENTOS INSTANTÂNEOS (Triggers)
    //==========================================================

    /// <summary>
    /// Disparado sempre que um salto normal é executado.
    /// </summary>
    public event Action JumpTriggered;

    /// <summary>
    /// Disparado sempre que um Double Jump é executado.
    /// </summary>
    public event Action DoubleJumpTriggered;

    /// <summary>
    /// Disparado quando um Dash começa.
    /// </summary>
    public event Action DashTriggered;

    /// <summary>
    /// Disparado quando um ataque é executado.
    /// </summary>
    public event Action AttackTriggered;

    /// <summary>
    /// Disparado quando o personagem sofre dano.
    /// </summary>
    public event Action HurtTriggered;

    // /// <summary>
    // /// Disparado quando o personagem toca o chão.
    // /// </summary>
    // public event Action Landed;

    //==========================================================
    // SETTERS DOS ESTADOS CONTÍNUOS
    //==========================================================

    public void SetGrounded(bool value)
    {
        if (IsGrounded == value)
            return;

        IsGrounded = value;

        GroundedChanged?.Invoke(value);

        // if (value)
        //     Landed?.Invoke();
    }

    public void SetRunning(bool value)
    {
        if (IsRunning == value)
            return;

        IsRunning = value;

        RunningChanged?.Invoke(value);
    }

    public void SetRising(bool value)
    {
        if (IsRising == value)
            return;

        IsRising = value;

        RisingChanged?.Invoke(value);
    }

    public void SetFalling(bool value)
    {
        if (IsFalling == value)
            return;

        IsFalling = value;

        FallingChanged?.Invoke(value);
    }

    public void SetWallSliding(bool value)
    {
        if (IsWallSliding == value)
            return;

        IsWallSliding = value;

        WallSlidingChanged?.Invoke(value);
    }

    public void SetDashing(bool value)
    {
        if (IsDashing == value)
            return;

        IsDashing = value;

        DashingChanged?.Invoke(value);
    }

    /// <summary>
    /// Habilita ou desabilita o controle horizontal do personagem.
    /// </summary>
    public void SetCanMoveHorizontally(bool value)
    {
        if (HasMovementControl == value)
            return;

        HasMovementControl = value;

        CanMoveHorizontallyChanged?.Invoke(value);
    }

    public void SetFacingDirection(int value)
    {
        if (FacingDirection == value)
            return;

        FacingDirection = value;

        FacingDirectionChanged?.Invoke(value);
    }

    //==========================================================
    // TRIGGERS
    //==========================================================

    public void TriggerJump()
    {
        JumpTriggered?.Invoke();
    }

    public void TriggerDoubleJump()
    {
        DoubleJumpTriggered?.Invoke();
    }

    public void TriggerDash()
    {
        DashTriggered?.Invoke();
    }

    public void TriggerAttack()
    {
        AttackTriggered?.Invoke();
    }

    public void TriggerHurt()
    {
        HurtTriggered?.Invoke();
    }
}