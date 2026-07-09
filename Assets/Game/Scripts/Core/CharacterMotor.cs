using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CharacterState))]
[RequireComponent(typeof(GroundDetector))]
public class CharacterMotor : MonoBehaviour
{
    private Rigidbody2D rb;
    private CharacterState state;

    [SerializeField] private GroundDetector groundDetector;

    private float horizontalControlLockTimer;

    // private float currentSpeed;
    
    
    public bool CanControlHorizontalMovement => horizontalControlLockTimer <= 0f;


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

        if (horizontalControlLockTimer > 0f)
        {
            horizontalControlLockTimer -= Time.fixedDeltaTime;
        }
    }

    private void UpdateGroundState()
    {
        state.SetGrounded(groundDetector.IsGrounded);
    }

    private void UpdateVerticalStates()
    {
        float verticalVelocity = rb.linearVelocity.y;

        const float threshold = 0.05f;

        state.SetRising(verticalVelocity > threshold);

        state.SetFalling(verticalVelocity < threshold);
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

    /// <summary>
    /// Bloqueia temporariamente o controle horizontal.
    /// </summary>
    public void LockHorizontalControl(float duration)
    {
        horizontalControlLockTimer = Mathf.Max(horizontalControlLockTimer, duration);
    }

}