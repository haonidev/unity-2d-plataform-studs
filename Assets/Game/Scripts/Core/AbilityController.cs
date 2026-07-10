using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Orquestra o ciclo de vida das habilidades do personagem.
/// Descobre todas as abilities no GameObject e as executa nos ciclos corretos do Unity.
/// </summary>
[RequireComponent(typeof(CharacterContext))]
public class AbilityController : MonoBehaviour
{
    private readonly List<IAbility> abilities = new();

    private CharacterContext context;

    /// <summary>
    /// Inicializa o controlador e registra todas as habilidades encontradas no mesmo GameObject.
    /// </summary>
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

    /// <summary>
    /// Executa o Tick de cada habilidade e reseta os eventos transitórios do frame atual.
    /// </summary>
    private void Update()
    {
        foreach (var ability in abilities)
        {
            ability.Tick();
        }
        context.FrameInput.ResetFrameActions();
    }

    /// <summary>
    /// Executa o FixedTick de cada habilidade no ciclo físico.
    /// </summary>
    private void FixedUpdate()
    {
        foreach (var ability in abilities)
        {
            ability.FixedTick();
        }
    }
}