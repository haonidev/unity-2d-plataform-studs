using UnityEngine;

/// <summary>
/// Detecta se o personagem está tocando o chão através de um overlap circle.
/// </summary>
[RequireComponent(typeof(CharacterContext))]
public class GroundDetector : MonoBehaviour
{
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float radius = 0.18f;
    [SerializeField] private LayerMask groundLayer;

    private bool lastGrounded;

    /// <summary>
    /// Indica se o personagem está atualmente em contato com o chão.
    /// </summary>
    public bool IsGrounded { get; private set; }

    /// <summary>
    /// Atualiza o estado de grounded no ciclo físico usando a camada configurada.
    /// </summary>
    private void FixedUpdate()
    {
        IsGrounded = Physics2D.OverlapCircle(
            groundCheck.position,
            radius,
            groundLayer);

        if (IsGrounded != lastGrounded)
        {
            lastGrounded = IsGrounded;
        }

        IsGrounded = lastGrounded;
    }

#if UNITY_EDITOR
    /// <summary>
    /// Desenha o raio de detecção no editor para facilitar o ajuste da colisão.
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        if (groundCheck == null)
            return;

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(groundCheck.position, radius);
    }
#endif
}