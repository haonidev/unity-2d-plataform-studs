using UnityEngine;

/// <summary>
/// Contrato para objetos que podem receber efeitos de uma trap.
/// A implementação no alvo decide como encaminhar o efeito ao seu próprio gameplay.
/// </summary>
public abstract class TrapTarget : MonoBehaviour
{
    /// <summary>
    /// Solicita que o alvo receba um efeito letal.
    /// </summary>
    public abstract void ReceiveLethalTrap();
}
