using System;
using UnityEngine;

public class CharacterState : MonoBehaviour
{
    /// <summary>
    /// Indica se o personagem está deslizando na parede.
    /// </summary>
    public bool IsWallSliding { get; private set; }

    /// <summary>
    /// Indica se o personagem está executando um dash.
    /// </summary>
    public bool IsDashing { get; private set; }

    /// <summary>
    /// Indica se o personagem está atacando.
    /// </summary>
    public bool IsAttacking { get; private set; }

    /// <summary>
    /// Atualiza o estado de Wall Slide.
    /// </summary>
    public void SetWallSliding(bool value)
    {
        if (IsWallSliding == value)
            return;

        IsWallSliding = value;

        WallSlidingChanged?.Invoke(value);
    }

    /// <summary>
    /// Atualiza o estado de Dash.
    /// </summary>
    public void SetDashing(bool value)
    {
        if (IsDashing == value)
            return;

        IsDashing = value;

        DashingChanged?.Invoke(value);
    }

    /// <summary>
    /// Atualiza o estado de Ataque.
    /// </summary>
    public void SetAttacking(bool value)
    {
        if (IsAttacking == value)
            return;

        IsAttacking = value;

        AttackingChanged?.Invoke(value);
    }

    /// <summary>
    /// Disparado quando o estado de Wall Slide muda.
    /// </summary>
    public event Action<bool> WallSlidingChanged;

    /// <summary>
    /// Disparado quando o estado de Dash muda.
    /// </summary>
    public event Action<bool> DashingChanged;

    /// <summary>
    /// Disparado quando o estado de Ataque muda.
    /// </summary>
    public event Action<bool> AttackingChanged;
}