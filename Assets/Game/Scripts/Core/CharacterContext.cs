using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(CharacterMotor))]
[RequireComponent(typeof(CharacterState))]
[RequireComponent(typeof(GroundDetector))]
[RequireComponent(typeof(WallDetector))]
[RequireComponent(typeof(PlayerInputReader))]
public class CharacterContext : MonoBehaviour
{
    public Rigidbody2D Rigidbody { get; private set; }
    public CharacterMotor Motor { get; private set; }
    private WallDetector wallDetector;
    private GroundDetector groundDetector;
    private PlayerInputReader input;
    private CharacterState state;

    private void Initialize()
    {
    }

    private void Awake()
    {
        CacheComponents();
        ValidateComponents();
    }

    private void CacheComponents()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
        Motor = GetComponent<CharacterMotor>();

        wallDetector = GetComponent<WallDetector>();
        groundDetector = GetComponent<GroundDetector>();
        input = GetComponent<PlayerInputReader>();
        state = GetComponent<CharacterState>();
    }

    private void ValidateComponents()
    {
        Debug.Assert(Rigidbody != null);
        Debug.Assert(Motor != null);
        Debug.Assert(wallDetector != null);
        Debug.Assert(groundDetector != null);
        Debug.Assert(input != null);

        Debug.Assert(state != null);
    }

    // -------------------------
    // 🎮 API DE MOVIMENTO
    // -------------------------


    /// <summary>
    /// Estado do input do frame atual.
    /// </summary>
    public PlayerFrameInput FrameInput => input.FrameInput;
    
    /// <summary>
    /// Entrada de movimento.
    /// </summary>
    public Vector2 MoveInput => FrameInput.Move;

    /// <summary>
    /// Botão de salto pressionado neste frame.
    /// </summary>
    public bool JumpPressed => FrameInput.JumpPressed;

    /// <summary>
    /// Botão de salto liberado neste frame.
    /// </summary>
    public bool JumpReleased => FrameInput.JumpReleased;


    /// <summary>
    /// Limpa todos os eventos de entrada de um único frame.
    /// Deve ser chamado apenas pelo AbilityController.
    /// </summary>
    public void ClearFrameInputs()
    {
        input.ClearFrameInputs();
    }

    // -------------------------
    // 🌍 ESTADO DO MUNDO
    // -------------------------

    public bool IsGrounded => groundDetector.IsGrounded;

    public bool IsTouchingWall => wallDetector.IsTouchingWall;

    public int WallDirection => wallDetector.WallDirection;

    /// <summary>
    /// Velocidade vertical atual.
    /// </summary>
    public float VerticalVelocity => Motor.GetVerticalVelocity();

    /// <summary>
    /// Indica se o personagem está subindo.
    /// </summary>
    public bool IsRising => VerticalVelocity > 0f;

    /// <summary>
    /// Indica se o personagem está caindo.
    /// </summary>
    public bool IsFalling => VerticalVelocity < 0f;

    public CharacterState State => state;
}