using UnityEngine;

/// <summary>
/// Detecta contato com paredes laterais para habilitar habilidades como wall slide e wall jump.
/// </summary>
[RequireComponent(typeof(CharacterContext))]
public class WallDetector : MonoBehaviour
{
    [Header("Wall Check")]

    [SerializeField]
    private Transform leftWallCheck;

    [SerializeField]
    private Transform rightWallCheck;

    [SerializeField]
    private float radius = 0.04f;

    [SerializeField]
    private LayerMask wallLayer;

    /// <summary>
    /// Indica se o personagem está tocando uma parede.
    /// </summary>
    public bool IsTouchingWall { get; private set; }

    /// <summary>
    /// Direção da parede detectada: -1 para esquerda, 0 para nenhuma e 1 para direita.
    /// </summary>
    public int WallDirection { get; private set; }

    /// <summary>
    /// Atualiza o estado de contato com parede no ciclo físico.
    /// </summary>
    private void FixedUpdate()
    {
        Collider2D leftCollider = Physics2D.OverlapCircle(
            leftWallCheck.position,
            radius,
            wallLayer);

        Collider2D rightCollider = Physics2D.OverlapCircle(
            rightWallCheck.position,
            radius,
            wallLayer);

        bool left = leftCollider != null;
        bool right = rightCollider != null;

        if (left)
        {
            IsTouchingWall = true;
            WallDirection = -1;
        }
        else if (right)
        {
            IsTouchingWall = true;
            WallDirection = 1;
        }
        else
        {
            IsTouchingWall = false;
            WallDirection = 0;
        }
    }

#if UNITY_EDITOR

    /// <summary>
    /// Desenha os pontos de verificação de parede no editor.
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;

        if (leftWallCheck != null)
            Gizmos.DrawWireSphere(leftWallCheck.position, radius);

        if (rightWallCheck != null)
            Gizmos.DrawWireSphere(rightWallCheck.position, radius);
    }

#endif
}