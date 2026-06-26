using UnityEngine;

[DisallowMultipleComponent]
public class CharacterContext : MonoBehaviour
{
    public Rigidbody2D Rigidbody { get; private set; }
    public CharacterMotor Motor { get; private set; }

    private GroundDetector groundDetector;
    private PlayerInputReader input;

    private void Awake()
    {
        CacheComponents();
        ValidateComponents();
    }

    private void CacheComponents()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
        Motor = GetComponent<CharacterMotor>();

        groundDetector = GetComponent<GroundDetector>();
        input = GetComponent<PlayerInputReader>();
    }

    private void ValidateComponents()
    {
        Debug.Assert(Rigidbody != null);
        Debug.Assert(Motor != null);
        Debug.Assert(groundDetector != null);
        Debug.Assert(input != null);
    }

    // -------------------------
    // 🎮 API DE MOVIMENTO
    // -------------------------

    public Vector2 MoveInput => input.MoveInput;

    //public bool JumpPressed => input.JumpPressed;

    public bool ConsumeJumpPressed()
    {
        return input.ConsumeJumpPressed();
    }

    public bool ConsumeJumpReleased()
    {
        return input.ConsumeJumpReleased();
    }

    // -------------------------
    // 🌍 ESTADO DO MUNDO
    // -------------------------

    public bool IsGrounded => groundDetector.IsGrounded;
}