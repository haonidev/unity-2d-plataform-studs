using UnityEngine;

/// <summary>
/// Responsável por restaurar o personagem após a conclusão da sequência de morte.
/// </summary>
[RequireComponent(typeof(CharacterContext))]
public class RespawnController : MonoBehaviour
{
    [SerializeField]
    private RespawnPoint currentRespawnPoint;

    private CharacterContext context;

    private enum RespawnPhase
    {
        Idle,
        WaitingAnimation
    }

    private RespawnPhase phase = RespawnPhase.Idle;

    private void Awake()
    {
        context = GetComponent<CharacterContext>();

        Debug.Assert(context != null);
        Debug.Assert(currentRespawnPoint != null);
    }

    private void Update()
    {
        if (phase != RespawnPhase.WaitingAnimation)
            return;

        var state = GetComponent<Animator>().GetCurrentAnimatorStateInfo(0);

        if (state.IsName("Player_respawn") && state.normalizedTime >= 1f)
        {
            Debug.Log("RESPAWN FINALIZADO VIA POLLING");

            OnRespawnAnimationFinished();
        }
    }

    private void OnEnable()
    {
        context ??= GetComponent<CharacterContext>();

        Debug.Assert(context != null);
        Debug.Assert(context.State != null);

        context.State.DeathCompleted += OnDeathCompleted;
        context.State.RespawnAnimationFinished += OnRespawnAnimationFinished;
    }

    private void OnDisable()
    {
        if (context == null)
            return;

        context.State.DeathCompleted -= OnDeathCompleted;
        context.State.RespawnAnimationFinished -= OnRespawnAnimationFinished;
    }

    private void OnRespawnAnimationFinished()
    {
        Debug.Log("[RespawnController] RespawnAnimationFinished");
        if (phase != RespawnPhase.WaitingAnimation)
        {
            Debug.Log("[RespawnController] phase != RespawnPhase.WaitingAnimation -> True;");
            
            return;
        }

        phase = RespawnPhase.Idle;

        context.State.SetHasControl(true);

        context.State.CompleteRespawn();
    }

    private void OnDeathCompleted()
    {
        Respawn();
    }

    /// <summary>
    /// "O personagem terminou de renascer."
    /// </summary>
    private void Respawn()
    {
        phase = RespawnPhase.WaitingAnimation;

        context.Motor.ResetMotion();
        context.State.ResetGameplayState();

        transform.position = currentRespawnPoint.transform.position;

        context.State.TriggerRespawn();

    }
        
    /// <summary>
    /// Define o ponto de respawn atual.
    /// </summary>
    public void SetRespawnPoint(RespawnPoint respawnPoint)
    {
        Debug.Assert(respawnPoint != null);

        currentRespawnPoint = respawnPoint;
    }

}