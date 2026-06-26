using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class CharacterMotor : MonoBehaviour
{
    private Rigidbody2D rb;

    [SerializeField] private int maxAirJumps = 1;
    private int remainingAirJumps;

    private bool wasGrounded;


    [SerializeField] private GroundDetector groundDetector;
    public bool IsGrounded => groundDetector.IsGrounded;


    private float currentSpeed;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        remainingAirJumps = maxAirJumps;
    }

    private void Update()
    {
        CheckGroundState();
    }

    private void FixedUpdate()
    {
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

    private void CheckGroundState()
    {
        if (IsGrounded && !wasGrounded)
        {
            OnLanded();
        }

        wasGrounded = IsGrounded;
    }

    /// <summary>
    /// Evento de aterrissagem
    /// </summary>
    private void OnLanded()
    {
        remainingAirJumps = maxAirJumps;
    }

    public bool TryJump(float jumpForce)
    {
        if (IsGrounded)
        {
            SetVerticalVelocity(jumpForce);
            return true;
        }

        if (remainingAirJumps > 0)
        {
            SetVerticalVelocity(jumpForce);
            remainingAirJumps--;
            return true;
        }

        return false;
    }
}