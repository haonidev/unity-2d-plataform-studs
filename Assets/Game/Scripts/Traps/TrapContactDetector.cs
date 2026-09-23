using System;
using UnityEngine;

/// <summary>
/// Detecta alvos entrando na área de trigger da trap e publica a ativação.
/// </summary>
[DisallowMultipleComponent]
public sealed class TrapContactDetector : MonoBehaviour
{
    public event Action<TrapTarget> Activated;

    private void Reset()
    {
        Collider2D trigger = GetComponent<Collider2D>();
        if (trigger != null)
            trigger.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        TrapTarget target = other.GetComponentInParent<TrapTarget>();
        if (target == null)
            return;

        Activated?.Invoke(target);
    }
}
