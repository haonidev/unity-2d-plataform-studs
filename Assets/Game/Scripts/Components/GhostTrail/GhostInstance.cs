using System;
using UnityEngine;

/// <summary>
/// Representa um único ghost utilizado pelo efeito de Ghost Trail.
///
/// Esta classe possui apenas uma responsabilidade:
/// exibir uma cópia visual do personagem durante um curto período
/// e informar quando terminou sua animação.
///
/// Ela não conhece:
/// - Player;
/// - DashAbility;
/// - GhostPool;
/// - CharacterState.
///
/// Toda a configuração é recebida através de um GhostSnapshot.
/// </summary>
[RequireComponent(typeof(SpriteRenderer))]
public class GhostInstance : MonoBehaviour
{
    //==========================================================
    // EVENTOS
    //==========================================================

    /// <summary>
    /// Disparado quando o ghost terminou sua animação.
    /// O Pool será responsável por reutilizar esta instância.
    /// </summary>
    public event Action<GhostInstance> Finished;

    //==========================================================
    // REFERENCES
    //==========================================================

    [Header("References")]
    [SerializeField]
    private SpriteRenderer spriteRenderer;

    //==========================================================
    // RUNTIME
    //==========================================================

    /// <summary>
    /// Tempo total do fade.
    /// </summary>
    private float duration;

    /// <summary>
    /// Tempo restante.
    /// </summary>
    private float timer;

    /// <summary>
    /// Cor inicial utilizada para preservar o RGB
    /// enquanto apenas o Alpha é alterado.
    /// </summary>
    private Color initialColor;

    /// <summary>
    /// Indica se este ghost está ativo.
    /// Evita executar Update quando estiver no pool.
    /// </summary>
    private bool isRunning;

    //==========================================================
    // UNITY
    //==========================================================

    /// <summary>
    /// Restaura o Ghost para seu estado inicial, permitindo sua reutilização
    /// pelo GhostPool.
    /// </summary>
    public void ResetState()
    {
        duration = 0f;
        timer = 0f;

        isRunning = false;

        initialColor = Color.white;

        spriteRenderer.sprite = null;
        spriteRenderer.flipX = false;
        spriteRenderer.color = Color.white;
    }

    private void Reset()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Awake()
    {
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnDisable()
    {
        isRunning = false;
    }

    private void Update()
    {
        if (!isRunning)
            return;

        timer -= Time.deltaTime;

        float progress = 1f - Mathf.Clamp01(timer / duration);

        UpdateAlpha(progress);

        if (timer > 0f)
            return;

        isRunning = false;

        Finished?.Invoke(this);
    }

    //==========================================================
    // PUBLIC API
    //==========================================================

    /// <summary>
    /// Inicializa o ghost utilizando uma captura visual do jogador.
    /// </summary>
    public void Initialize(GhostSnapshot snapshot, float fadeDuration)
    {
        duration = fadeDuration;
        timer = fadeDuration;

        transform.position = snapshot.Position;
        transform.rotation = snapshot.Rotation;
        transform.localScale = snapshot.Scale;

        spriteRenderer.sprite = snapshot.Sprite;
        spriteRenderer.flipX = snapshot.FlipX;

        spriteRenderer.color = snapshot.Color;
        initialColor = snapshot.Color;

        spriteRenderer.sortingLayerID = snapshot.SortingLayerID;
        spriteRenderer.sortingOrder = snapshot.SortingOrder;

        isRunning = true;
    }

    //==========================================================
    // PRIVATE
    //==========================================================

    /// <summary>
    /// Atualiza somente o canal Alpha da cor.
    /// O RGB permanece exatamente igual ao sprite original.
    /// </summary>
    private void UpdateAlpha(float progress)
    {
        Color color = initialColor;

        color.a = Mathf.Lerp(initialColor.a, 0f, progress);

        spriteRenderer.color = color;
    }
}