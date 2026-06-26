using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterContext))]
public class AbilityController : MonoBehaviour
{
    private readonly List<IAbility> abilities = new();

    private void Awake()
    {
        Debug.Log("AbilityController Awake");

        var _abilities = GetComponents<Ability>();

        Debug.Log("Abilities encontradas: " + _abilities.Length);

        foreach (var ability in _abilities)
        {
            Debug.Log("Ability: " + ability.GetType().Name);

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