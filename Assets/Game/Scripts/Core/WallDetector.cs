using UnityEngine;

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

    public bool IsTouchingWall { get; private set; }

    /// <summary>
    /// -1 = parede à esquerda
    /// 0 = nenhuma parede
    /// 1 = parede à direita
    /// </summary>
    public int WallDirection { get; private set; }

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