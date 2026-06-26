using UnityEngine;

public abstract class Ability : MonoBehaviour, IAbility
{
    protected CharacterContext Context { get; private set; }

    public virtual void Initialize()
    {
        Context = GetComponent<CharacterContext>();

        Debug.Assert(
            Context != null,
            $"{GetType().Name}: CharacterContext não encontrado."
        );
    }

    public virtual void Tick() { }

    public virtual void FixedTick() { }
}