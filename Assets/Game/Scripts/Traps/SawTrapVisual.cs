using UnityEngine;

/// <summary>
/// Exibe o sprite Off quando inerte e percorre os frames On enquanto ativa.
/// </summary>
[DisallowMultipleComponent]
public sealed class SawTrapVisual : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite idleSprite;
    [SerializeField] private Sprite[] activeFrames;

    [SerializeField, Min(1f)]
    private float framesPerSecond = 12f;

    private bool isActive;
    private int currentFrame;
    private float frameTimer;

    private void Awake()
    {
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();

        ApplyCurrentSprite();
    }

    public void SetActive(bool active)
    {
        isActive = active;
        currentFrame = 0;
        frameTimer = 0f;
        ApplyCurrentSprite();
    }

    private void Update()
    {
        if (!isActive || spriteRenderer == null || activeFrames == null || activeFrames.Length == 0)
            return;

        float frameDuration = 1f / framesPerSecond;
        frameTimer += Time.deltaTime;

        while (frameTimer >= frameDuration)
        {
            frameTimer -= frameDuration;
            currentFrame = (currentFrame + 1) % activeFrames.Length;
        }

        ApplyCurrentSprite();
    }

    private void ApplyCurrentSprite()
    {
        if (spriteRenderer == null)
            return;

        bool hasActiveFrames = activeFrames != null && activeFrames.Length > 0;
        spriteRenderer.sprite = isActive && hasActiveFrames
            ? activeFrames[currentFrame % activeFrames.Length]
            : idleSprite;
    }

    private void OnValidate()
    {
        framesPerSecond = Mathf.Max(1f, framesPerSecond);
    }
}
