using UnityEngine;

/// <summary>
/// Adapta o contrato de traps ao fluxo de morte já existente no personagem.
/// </summary>
[RequireComponent(typeof(DeathAbility))]
public sealed class PlayerTrapTarget : TrapTarget
{
    private DeathAbility deathAbility;

    private void Awake()
    {
        deathAbility = GetComponent<DeathAbility>();
    }

    public override void ReceiveLethalTrap()
    {
        if (deathAbility == null)
            return;

        deathAbility.RequestDeath();
    }
}
