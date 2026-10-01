using System;
using UnityEngine;

/// <summary>
/// Mantém o estado ativo da serra e sincroniza visual, colisão e movimento.
/// </summary>
[DisallowMultipleComponent]
public sealed class SawTrapController : MonoBehaviour
{
    [Header("Initial State")]
    [Tooltip("Estado ao iniciar a cena. Desativada começa com Off e sem área letal.")]
    [SerializeField] private bool initiallyActive;

    [Header("Movement")]
    [Tooltip("Se habilitado, a serra se move pelo percurso configurado enquanto ativa.")]
    [SerializeField] private bool movesWhenActive;

    [Header("References")]
    [SerializeField] private SawTrapVisual visual;
    [SerializeField] private Collider2D hazardCollider;
    [SerializeField] private SawTrapMovement movement;

    private bool isActive;

    public bool IsActive => isActive;
    public event Action<bool> ActiveChanged;

    private void Awake()
    {
        if (visual == null)
            visual = GetComponentInChildren<SawTrapVisual>();

        if (hazardCollider == null)
            hazardCollider = GetComponent<Collider2D>();

        if (movement == null)
            movement = GetComponent<SawTrapMovement>();

        isActive = initiallyActive;
    }

    private void Start()
    {
        ApplyState(isActive);
    }

    /// <summary>
    /// Ativa a serra. Pode ser chamado por triggers e por outros sistemas de gameplay.
    /// </summary>
    public void Activate() => SetActive(true);

    /// <summary>
    /// Desativa a serra.
    /// </summary>
    public void Deactivate() => SetActive(false);

    /// <summary>
    /// Altera o estado da serra e notifica ouvintes apenas quando ele muda.
    /// </summary>
    public void SetActive(bool active)
    {
        if (isActive == active)
            return;

        isActive = active;
        ApplyState(active);
        ActiveChanged?.Invoke(active);
    }

    private void ApplyState(bool active)
    {
        if (visual != null)
            visual.SetActive(active);

        if (hazardCollider != null)
            hazardCollider.enabled = active;

        if (movement != null)
            movement.enabled = active && movesWhenActive;
    }

    private void OnValidate()
    {
        if (Application.isPlaying)
            return;

        if (hazardCollider != null)
            hazardCollider.enabled = initiallyActive;

        if (movement != null)
            movement.enabled = initiallyActive && movesWhenActive;
    }
}
