using UnityEngine;

[RequireComponent(typeof(CharacterContext))]
public class GroundDetector : MonoBehaviour
{
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float radius = 0.18f;
    [SerializeField] private LayerMask groundLayer;

    private bool lastGrounded;
    public bool IsGrounded { get; private set; }

    private void FixedUpdate()
    {
        IsGrounded = Physics2D.OverlapCircle(
            groundCheck.position,
            radius,
            groundLayer);

        Debug.Log($"GroundDetector: {IsGrounded}");

        // 🔥 filtro de estabilidade
        if (IsGrounded != lastGrounded)
        {
            lastGrounded = IsGrounded;
        }

        IsGrounded = lastGrounded;
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        if (groundCheck == null)
            return;

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(groundCheck.position, radius);
    }
#endif
}