using UnityEngine;

/// <summary>
/// Representa uma captura ("snapshot") do estado visual do personagem
/// em um determinado instante.
///
/// É utilizado pelo sistema de Ghost Trail para reproduzir exatamente
/// a aparência do jogador sem manter nenhuma referência ao Player.
/// </summary>
public readonly struct GhostSnapshot
{
    /// <summary>
    /// Sprite exibido naquele frame.
    /// </summary>
    public readonly Sprite Sprite;

    /// <summary>
    /// Posição do personagem.
    /// </summary>
    public readonly Vector3 Position;

    /// <summary>
    /// Rotação do personagem.
    /// </summary>
    public readonly Quaternion Rotation;

    /// <summary>
    /// Escala do personagem.
    /// </summary>
    public readonly Vector3 Scale;

    /// <summary>
    /// Indica se o sprite está invertido horizontalmente.
    /// </summary>
    public readonly bool FlipX;

    /// <summary>
    /// Cor utilizada pelo SpriteRenderer.
    /// </summary>
    public readonly Color Color;

    /// <summary>
    /// Sorting Layer utilizada.
    /// </summary>
    public readonly int SortingLayerID;

    /// <summary>
    /// Ordem dentro da Sorting Layer.
    /// </summary>
    public readonly int SortingOrder;

    public GhostSnapshot(
        Sprite sprite,
        Vector3 position,
        Quaternion rotation,
        Vector3 scale,
        bool flipX,
        Color color,
        int sortingLayerID,
        int sortingOrder)
    {
        Sprite = sprite;
        Position = position;
        Rotation = rotation;
        Scale = scale;
        FlipX = flipX;
        Color = color;
        SortingLayerID = sortingLayerID;
        SortingOrder = sortingOrder;
    }
}