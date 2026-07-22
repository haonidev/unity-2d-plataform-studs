using UnityEngine;

/// <summary>
/// Coordena a sequência de morte do personagem.
/// </summary>
public class DeathAbility : Ability
{
    private enum DeathPhase
    {
        Idle,
        WaitingAnimation
    }

    private DeathPhase phase = DeathPhase.Idle;

    public override void Initialize()
    {
        base.Initialize();

        Context.State.DeathAnimationFinished += OnDeathAnimationFinished;
    }

    private void OnDestroy()
    {
        if (Context != null)
        {
            Context.State.DeathAnimationFinished -= OnDeathAnimationFinished;
        }
    }

    /// <summary>
    /// Solicita o início da sequência de morte.
    /// </summary>
    public void RequestDeath()
    {
        if (!Context.State.HasControl)
                return;

        StartDeathSequence();
    }

    /// <summary>
    /// Inicia a sequência de morte.
    /// </summary>
    private void StartDeathSequence()
    {
        Debug.Log("Death sequence started.");

        phase = DeathPhase.WaitingAnimation;

        Context.State.SetHasControl(false);

        Context.Motor.SetHorizontalVelocity(0f);

        Context.State.TriggerDeath();
    }

    /// <summary>
    /// Finaliza a sequência após o término da animação.
    /// </summary>
    private void OnDeathAnimationFinished()
    {
        if (phase != DeathPhase.WaitingAnimation)
            return;

        phase = DeathPhase.Idle;

        Context.State.CompleteDeath();
    }
}