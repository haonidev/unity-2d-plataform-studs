using UnityEngine;

/// <summary>
/// Ativa uma serra quando um alvo reconhecido entra na área deste trigger.
/// </summary>
[DisallowMultipleComponent]
public sealed class SawActivationTrigger : MonoBehaviour
{
    [Tooltip("Serra que será ativada quando o jogador entrar neste trigger.")]
    [SerializeField] private SawTrapController target;

    private void Reset()
    {
        Collider2D trigger = GetComponent<Collider2D>();
        if (trigger != null)
            trigger.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (target == null || other.GetComponentInParent<TrapTarget>() == null)
            return;

        target.Activate();
    }
}
