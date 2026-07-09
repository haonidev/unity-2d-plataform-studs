using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterContext))]
public class AbilityController : MonoBehaviour
{
    private readonly List<IAbility> abilities = new();

    private CharacterContext context;

    private void Awake()
    {
        context = GetComponent<CharacterContext>();

        Debug.Assert(context != null);

        var abilityComponents = GetComponents<Ability>();

        foreach (var ability in abilityComponents)
        {
            ability.Initialize();
            abilities.Add(ability);
        }
    }

    private void Update()
    {
        foreach (var ability in abilities)
        {
            ability.Tick();
        }
        context.FrameInput.ResetFrameActions();
    }

    private void FixedUpdate()
    {
        foreach (var ability in abilities)
        {
            ability.FixedTick();
        }
    }
}