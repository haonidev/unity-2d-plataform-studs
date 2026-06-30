using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterContext))]
public class AbilityController : MonoBehaviour
{
    private readonly List<IAbility> abilities = new();

    private void Awake()
    {
        var _abilities = GetComponents<Ability>();

        foreach (var ability in _abilities)
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
    }

    private void FixedUpdate()
    {
        foreach (var ability in abilities)
        {
            ability.FixedTick();
        }
    }
}