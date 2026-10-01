/// <remarks>
/// Ferramenta temporária utilizada apenas durante o desenvolvimento.
/// Deve ser removida quando os sistemas definitivos de gameplay estiverem implementados.
/// </remarks>

using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Debugging
{
    public class GameplayDebugInput : MonoBehaviour
    {
        [SerializeField]
        private DeathAbility deathAbility;

        private PlayerInputActions inputActions;

        private void Awake()
        {
            EnsureInputActions();

            if (deathAbility == null)
                deathAbility = GetComponent<DeathAbility>();

            Debug.Assert(
                deathAbility != null,
                $"{nameof(GameplayDebugInput)}: DeathAbility não encontrada.");
        }

        private void OnEnable()
        {
            EnsureInputActions();

            inputActions.Debug.Kill.performed += OnKill;
            inputActions.Enable();
        }

        private void OnDisable()
        {
            if (inputActions == null)
                return;

            inputActions.Debug.Kill.performed -= OnKill;

            inputActions.Disable();
        }

        private void OnDestroy()
        {
            inputActions?.Dispose();
            inputActions = null;
        }

        private void EnsureInputActions()
        {
            inputActions ??= new PlayerInputActions();
        }

        private void OnKill(InputAction.CallbackContext _)
        {
            deathAbility.RequestDeath();
        }
    }
}
