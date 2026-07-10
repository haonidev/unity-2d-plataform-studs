using UnityEngine;

/// <summary>
/// Centraliza o acesso aos componentes e aos dados do personagem para as habilidades e sistemas de gameplay.
/// Serve como ponto único de leitura de estado e entrada.
/// </summary>
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

    /// <summary>
    /// Inicializa a referência interna do contexto.
    /// TODO: este método pode ser removido futuramente se a inicialização for totalmente feita em Awake.
    /// </summary>
    private void Initialize()
    {
    }

    /// <summary>
    /// Cacheia os componentes principais necessários para o contexto do personagem.
    /// </summary>
    private void Awake()
    {
        CacheComponents();
        ValidateComponents();
    }

    /// <summary>
    /// Busca os componentes essenciais no mesmo GameObject.
    /// </summary>
    private void CacheComponents()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
        Motor = GetComponent<CharacterMotor>();

        wallDetector = GetComponent<WallDetector>();
        groundDetector = GetComponent<GroundDetector>();
        input = GetComponent<PlayerInputReader>();
        state = GetComponent<CharacterState>();
    }

    /// <summary>
    /// Garante que os componentes obrigatórios existam antes de usar o contexto.
    /// </summary>
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
    /// Botão de Dash pressionado neste frame.
    /// </summary>
    public bool DashPressed => FrameInput.DashPressed;

    /// <summary>
    /// Limpa os eventos de entrada de um único frame.
    /// Deve ser chamado apenas pelo AbilityController.
    /// </summary>
    public void ClearFrameInputs()
    {
        input.ClearFrameInputs();
    }

    // -------------------------
    // 🌍 ESTADO DO MUNDO
    // -------------------------

    /// <summary>
    /// Indica se o personagem está tocando o chão.
    /// </summary>
    public bool IsGrounded => groundDetector.IsGrounded;

    /// <summary>
    /// Indica se o personagem está tocando uma parede.
    /// </summary>
    public bool IsTouchingWall => wallDetector.IsTouchingWall;

    /// <summary>
    /// Direção da parede detectada, se houver.
    /// </summary>
    public int WallDirection => wallDetector.WallDirection;

    /// <summary>
    /// Velocidade vertical atual do personagem.
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

    /// <summary>
    /// Acesso ao estado de gameplay do personagem.
    /// </summary>
    public CharacterState State => state;
}