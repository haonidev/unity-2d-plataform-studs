using UnityEngine;

/// <summary>
/// Encapsula a aplicação das mudanças físicas do personagem no Rigidbody2D.
/// Centraliza velocidade, gravidade e estados derivados do movimento.
/// </summary>
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

    /// <summary>
    /// Indica se o personagem pode receber controle horizontal no momento.
    /// </summary>
    public bool CanControlHorizontalMovement => horizontalControlLockTimer <= 0f;

    /// <summary>
    /// Cacheia referências físicas e define a gravidade padrão inicial.
    /// </summary>
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        state = GetComponent<CharacterState>();
        groundDetector = GetComponent<GroundDetector>();

        defaultGravityScale = rb.gravityScale;
        requestedGravityScale = defaultGravityScale;
    }

    /// <summary>
    /// Atualiza estados de chão e vertical e aplica os pedidos de movimento e gravidade no ciclo físico.
    /// </summary>
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

    /// <summary>
    /// Aplica a velocidade horizontal solicitada ao Rigidbody.
    /// </summary>
    private void ApplyRequestedVelocity()
    {
        rb.linearVelocity = new Vector2(requestedHorizontalVelocity, rb.linearVelocity.y);

        requestedHorizontalVelocity = rb.linearVelocity.x;

        currentPriority = MotorPriority.Movement;
    }

    /// <summary>
    /// Aplica a gravidade solicitada ao Rigidbody.
    /// </summary>
    private void ApplyRequestedGravity()
    {
        rb.gravityScale = requestedGravityScale;

        requestedGravityScale = defaultGravityScale;

        gravityPriority = MotorPriority.Movement;
    }

    /// <summary>
    /// Sincroniza o estado de grounded com o GroundDetector.
    /// </summary>
    private void UpdateGroundState()
    {
        state.SetGrounded(groundDetector.IsGrounded);
    }

    /// <summary>
    /// Atualiza os estados de subida e queda com base na velocidade vertical atual.
    /// </summary>
    private void UpdateVerticalStates()
    {
        float verticalVelocity = rb.linearVelocity.y;

        const float threshold = 0.05f;

        state.SetRising(verticalVelocity > threshold);

        state.SetFalling(verticalVelocity < threshold);
    }

    /// <summary>
    /// Define a velocidade vertical do personagem.
    /// </summary>
    public void SetVerticalVelocity(float y)
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, y);
    }

    /// <summary>
    /// Define a velocidade horizontal do personagem.
    /// </summary>
    public void SetHorizontalVelocity(float x)
    {
        rb.linearVelocity = new Vector2(x, rb.linearVelocity.y);
    }

    /// <summary>
    /// Retorna a velocidade vertical atual do personagem.
    /// </summary>
    public float GetVerticalVelocity()
    {
        return rb.linearVelocity.y;
    }

    /// <summary>
    /// Solicita uma velocidade horizontal para ser aplicada no próximo FixedUpdate.
    /// </summary>
    public void RequestHorizontalVelocity(float velocity, MotorPriority priority)
    {
        if (priority < currentPriority)
            return;

        requestedHorizontalVelocity = velocity;

        currentPriority = priority;
    }

    /// <summary>
    /// Reduz a velocidade vertical atual por um multiplicador.
    /// </summary>
    public void ReduceVerticalVelocity(float multiplier)
    {
        var v = rb.linearVelocity;
        rb.linearVelocity = new Vector2(v.x, v.y / multiplier);
    }

    /// <summary>
    /// Solicita uma gravidade específica para ser aplicada com a prioridade informada.
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
    /// Limita a velocidade máxima de queda do personagem.
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
    /// Bloqueia temporariamente o controle horizontal do personagem por um tempo.
    /// </summary>
    public void LockHorizontalControl(float duration)
    {
        horizontalControlLockTimer = Mathf.Max(horizontalControlLockTimer, duration);
    }
}