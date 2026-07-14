using System.Collections;
using UnityEngine;

/// <summary>
/// Responsável por gerar o efeito de Ghost Trail durante o Dash.
///
/// Escuta o CharacterState e solicita Ghosts ao Pool
/// enquanto o personagem estiver em Dash.
/// </summary>
public class DashGhostTrail : MonoBehaviour
{
    [Header("References")]

    [SerializeField]
    private CharacterState state;

    [SerializeField]
    private SpriteRenderer spriteRenderer;

    [SerializeField]
    private GhostPool pool;

    [Header("Trail")]

    [SerializeField]
    [Min(0.01f)]
    private float interval = 0.03f;

    [SerializeField]
    [Min(0.01f)]
    private float fadeDuration = 0.20f;

    private Coroutine trailRoutine;

    //----------------------------------------------------------
    // UNITY
    //----------------------------------------------------------

    private void Reset()
    {
        state = GetComponent<CharacterState>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        state.DashingChanged += OnDashChanged;
    }

    private void OnDisable()
    {
        state.DashingChanged -= OnDashChanged;

        StopTrail();
    }

    //----------------------------------------------------------
    // EVENTS
    //----------------------------------------------------------

    private void OnDashChanged(bool isDashing)
    {
        if (isDashing)
        {
            StartTrail();
        }
        else
        {
            StopTrail();
        }
    }

    //----------------------------------------------------------
    // TRAIL
    //----------------------------------------------------------

    private void StartTrail()
    {
        if (trailRoutine != null)
            return;

        trailRoutine = StartCoroutine(TrailRoutine());
    }

    private void StopTrail()
    {
        if (trailRoutine == null)
            return;

        StopCoroutine(trailRoutine);

        trailRoutine = null;
    }

    private IEnumerator TrailRoutine()
    {
        WaitForSeconds wait =
            new(interval);

        while (true)
        {
            SpawnGhost();

            yield return wait;
        }
    }

    //----------------------------------------------------------
    // GHOST
    //----------------------------------------------------------

    private void SpawnGhost()
    {
        GhostInstance ghost = pool.Get();

        GhostSnapshot snapshot =
            CreateSnapshot();

        ghost.Initialize(snapshot, fadeDuration);
    }

    private GhostSnapshot CreateSnapshot()
    {
        Transform graphics = spriteRenderer.transform;

        return new GhostSnapshot(
            spriteRenderer.sprite,
            graphics.position,
            graphics.rotation,
            graphics.localScale,
            spriteRenderer.flipX,
            spriteRenderer.color,
            spriteRenderer.sortingLayerID,
            spriteRenderer.sortingOrder);
    }
}