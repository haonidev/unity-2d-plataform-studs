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

    private float requestedHorizontalVelocity;

    private MotorPriority currentPriority;

    private float defaultGravityScale;

    private float requestedGravityScale;

    private MotorPriority gravityPriority;
    
    
    public bool CanControlHorizontalMovement => horizontalControlLockTimer <= 0f;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        state = GetComponent<CharacterState>();
        groundDetector = GetComponent<GroundDetector>();

        defaultGravityScale = rb.gravityScale;
        requestedGravityScale = defaultGravityScale;
    }

    private void FixedUpdate()
    {
        UpdateGroundState();

        UpdateVerticalStates();

        if (horizontalControlLockTimer > 0f)
        {
            horizontalControlLockTimer -= Time.fixedDeltaTime;
        }

        ApplyRequestedVelocity();

        ApplyRequestedGravity();
    }

    private void ApplyRequestedVelocity()
    {
        rb.linearVelocity = new Vector2(requestedHorizontalVelocity, rb.linearVelocity.y);

        requestedHorizontalVelocity = rb.linearVelocity.x;

        currentPriority = MotorPriority.Movement;
    }

    private void ApplyRequestedGravity()
    {
        rb.gravityScale = requestedGravityScale;

        requestedGravityScale = defaultGravityScale;

        gravityPriority = MotorPriority.Movement;
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

    /// <summary>
    /// Solicita uma velocidade horizontal.
    /// A velocidade será aplicada no próximo FixedUpdate.
    /// </summary>
    public void RequestHorizontalVelocity(float velocity, MotorPriority priority)
    {
        if (priority < currentPriority)
            return;

        requestedHorizontalVelocity = velocity;

        currentPriority = priority;
    }

    public void ReduceVerticalVelocity(float multiplier)
    {
        var v = rb.linearVelocity;
        rb.linearVelocity = new Vector2(v.x, v.y / multiplier);
    }

    /// <summary>
    /// Solicita uma gravidade para este frame.
    /// Apenas a maior prioridade será aplicada.
    /// </summary>
    public void RequestGravityScale(
        float gravityScale,
        MotorPriority priority)
    {
        if (priority < gravityPriority)
            return;

        requestedGravityScale = gravityScale;

        gravityPriority = priority;
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