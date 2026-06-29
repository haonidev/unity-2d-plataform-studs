using UnityEngine;

public class VariableJumpDecorator : JumpDecorator
{
    [Header("Variable Jump")]
    [SerializeField]
    private float lowJumpMultiplier = 2.5f;

    public override void OnJumpReleased()
    {
        if (Ability.CharacterContext.Motor.GetVerticalVelocity() <= 0f)
            return;

        Ability.CharacterContext.Motor.ReduceVerticalVelocity(lowJumpMultiplier);
    }
}