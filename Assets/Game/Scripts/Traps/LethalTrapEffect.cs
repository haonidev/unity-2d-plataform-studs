using UnityEngine;

/// <summary>
/// Aplica o efeito letal quando um alvo reconhecido entra na área da trap.
/// </summary>
[RequireComponent(typeof(TrapContactDetector))]
public sealed class LethalTrapEffect : MonoBehaviour
{
    [Tooltip("Opcional: quando definido, o efeito só ocorre com esta Saw ativa.")]
    [SerializeField] private SawTrapController sawTrapController;

    private TrapContactDetector contactDetector;

    private void Awake()
    {
        contactDetector = GetComponent<TrapContactDetector>();

        if (sawTrapController == null)
            sawTrapController = GetComponent<SawTrapController>();
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
        if (target == null || (sawTrapController != null && !sawTrapController.IsActive))
            return;

        target.ReceiveLethalTrap();
    }
}
