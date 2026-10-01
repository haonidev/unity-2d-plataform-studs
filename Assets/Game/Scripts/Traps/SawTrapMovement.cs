using UnityEngine;

public enum SawMovementAxis
{
    Horizontal,
    Vertical
}

/// <summary>
/// Move a serra em um eixo, invertendo o sentido nos extremos do percurso.
/// O comprimento é a distância total entre os dois extremos, centrada na posição inicial.
/// </summary>
[DisallowMultipleComponent]
[RequireComponent(typeof(Rigidbody2D))]
public sealed class SawTrapMovement : MonoBehaviour
{
    [SerializeField] private SawMovementAxis axis = SawMovementAxis.Horizontal;

    [Tooltip("Comprimento total entre os extremos, centrado na posição inicial da serra.")]
    [SerializeField, Min(0.01f)]
    private float travelLength = 2f;

    [Tooltip("Velocidade em unidades do mundo por segundo.")]
    [SerializeField, Min(0f)]
    private float speed = 2f;

    private Rigidbody2D body;
    private Vector2 centerPosition;
    private float travelPhase;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        centerPosition = body.position;
        travelPhase = travelLength * 0.5f;
    }

    private void FixedUpdate()
    {
        if (travelLength <= 0f || speed <= 0f)
            return;

        travelPhase = Mathf.Repeat(
            travelPhase + speed * Time.fixedDeltaTime,
            travelLength * 2f);

        float offset = Mathf.PingPong(travelPhase, travelLength) - travelLength * 0.5f;
        Vector2 direction = axis == SawMovementAxis.Horizontal ? Vector2.right : Vector2.up;

        body.MovePosition(centerPosition + direction * offset);
    }

    private void OnValidate()
    {
        travelLength = Mathf.Max(0.01f, travelLength);
        speed = Mathf.Max(0f, speed);
    }

    private void OnDrawGizmosSelected()
    {
        Vector2 center = body != null ? body.position : (Vector2)transform.position;
        Vector2 direction = axis == SawMovementAxis.Horizontal ? Vector2.right : Vector2.up;
        float halfLength = Mathf.Max(0f, travelLength) * 0.5f;
        Vector2 start = center - direction * halfLength;
        Vector2 end = center + direction * halfLength;

        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(start, end);
        Gizmos.DrawWireSphere(start, 0.035f);
        Gizmos.DrawWireSphere(end, 0.035f);
    }
}
