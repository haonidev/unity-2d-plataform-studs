using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class CharacterMotor : MonoBehaviour
{
    private Rigidbody2D rb;
    private CharacterState state;

    [SerializeField] private GroundDetector groundDetector;

    private float currentSpeed;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        state = GetComponent<CharacterState>();
        groundDetector = GetComponent<GroundDetector>();
    }

    private void FixedUpdate()
    {
        UpdateGroundState();

        UpdateVerticalStates();
    }

    private void UpdateGroundState()
    {
        state.SetGrounded(groundDetector.IsGrounded);
    }

    private void UpdateVerticalStates()
    {
        float verticalVelocity = rb.linearVelocity.y;

        state.SetRising(verticalVelocity > 0f);

        state.SetFalling(verticalVelocity < 0f);
    }

    public void SetVerticalVelocity(float y)
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, y);
    }

    public void SetHorizontalVelocity(float x)
    {
        rb.linearVelocity = new Vector2(x, rb.linearVelocity.y);
    }

    public float GetVerticalVelocity()
    {
        return rb.linearVelocity.y;
    }

    public void ReduceVerticalVelocity(float multiplier)
    {
        var v = rb.linearVelocity;
        rb.linearVelocity = new Vector2(v.x, v.y / multiplier);
    }

    /// <summary>
    /// Limita a velocidade máxima de queda.
    /// </summary>
    public void ClampFallSpeed(float maxFallSpeed)
    {
        if (rb.linearVelocity.y < -maxFallSpeed)
        {
            rb.linearVelocity = new Vector2(
                rb.linearVelocity.x,
                -maxFallSpeed);
        }
    }
}