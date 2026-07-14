using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Gerencia a criação e reutilização das instâncias de Ghost.
/// </summary>
public class GhostPool : MonoBehaviour
{
    [Header("Pool")]

    [SerializeField]
    private GhostInstance prefab;

    [SerializeField]
    [Min(1)]
    private int initialSize = 8;

    private readonly Queue<GhostInstance> available =
        new();

    private void Awake()
    {
        for (int i = 0; i < initialSize; i++)
        {
            CreateGhost();
        }
    }

    /// <summary>
    /// Obtém uma instância disponível do pool.
    /// Caso não exista nenhuma livre, uma nova será criada.
    /// </summary>
    public GhostInstance Get()
    {
        if (available.Count == 0)
        {
            CreateGhost();
        }

        GhostInstance ghost = available.Dequeue();

        ghost.gameObject.SetActive(true);

        return ghost;
    }

    private GhostInstance CreateGhost()
    {
        GhostInstance ghost =
            Instantiate(prefab, transform);

        ghost.gameObject.SetActive(false);

        ghost.Finished += ReturnToPool;

        available.Enqueue(ghost);

        #if UNITY_EDITOR
            Debug.Log($"GhostPool: nova instância criada. Total disponível: {available.Count}");
        #endif

        return ghost;
    }

    private void ReturnToPool(GhostInstance ghost)
    {
        ghost.ResetState();

        ghost.gameObject.SetActive(false);

        available.Enqueue(ghost);
    }
};