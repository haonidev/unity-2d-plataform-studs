using UnityEngine;

/// <summary>
/// Classe base para todas as habilidades do personagem.
/// Fornece o contexto compartilhado e os hooks de ciclo de vida usados pelo AbilityController.
/// </summary>
public abstract class Ability : MonoBehaviour, IAbility
{
    protected CharacterContext Context { get; private set; }

    /// <summary>
    /// Inicializa a habilidade e resolve o CharacterContext associado ao GameObject.
    /// </summary>
    public virtual void Initialize()
    {
        Context = GetComponent<CharacterContext>();

        Debug.Assert(
            Context != null,
            $"{GetType().Name}: CharacterContext não encontrado."
        );
    }

    /// <summary>
    /// Executa a lógica principal da habilidade durante o Update.
    /// </summary>
    public virtual void Tick() { }

    /// <summary>
    /// Executa a lógica física da habilidade durante o FixedUpdate.
    /// </summary>
    public virtual void FixedTick() { }
}