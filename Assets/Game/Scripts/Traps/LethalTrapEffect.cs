using UnityEngine;

/// <summary>
/// Aplica o efeito letal quando um alvo reconhecido entra na área da trap.
/// </summary>
[RequireComponent(typeof(TrapContactDetector))]
public sealed class LethalTrapEffect : MonoBehaviour
{
    private TrapContactDetector contactDetector;

    private void Awake()
    {
        contactDetector = GetComponent<TrapContactDetector>();
    }

    private void OnEnable()
    {
        if (contactDetector == null)
            contactDetector = GetComponent<TrapContactDetector>();

        contactDetector.Activated += OnActivated;
    }

    private void OnDisable()
    {
        if (contactDetector != null)
            contactDetector.Activated -= OnActivated;
    }

    private void OnActivated(TrapTarget target)
    {
        target.ReceiveLethalTrap();
    }
}
