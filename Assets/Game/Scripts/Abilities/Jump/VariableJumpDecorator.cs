using UnityEngine;

/// <summary>
/// Reduz a força vertical do salto quando o botão de pulo é liberado cedo.
/// </summary>
public class VariableJumpDecorator : JumpDecorator
{
    [Header("Variable Jump")]
    [SerializeField]
    private float lowJumpMultiplier = 2.5f;

    /// <summary>
    /// Aplica a redução de velocidade vertical ao soltar o botão de pulo.
    /// </summary>
    public override void OnJumpReleased()
    {
        if (Ability.CharacterContext.Motor.GetVerticalVelocity() <= 0f)
            return;

        Ability.CharacterContext.Motor.ReduceVerticalVelocity(lowJumpMultiplier);
    }
}