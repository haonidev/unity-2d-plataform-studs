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
            inputActions = new PlayerInputActions();

            if (deathAbility == null)
                deathAbility = GetComponent<DeathAbility>();

            Debug.Assert(
                deathAbility != null,
                $"{nameof(GameplayDebugInput)}: DeathAbility não encontrada.");
        }

        private void OnEnable()
        {
            inputActions.Enable();

            inputActions.Debug.Kill.performed += OnKill;
        }

        private void OnDisable()
        {
            inputActions.Debug.Kill.performed -= OnKill;

            inputActions.Disable();
        }

        private void OnKill(InputAction.CallbackContext _)
        {
            deathAbility.RequestDeath();
        }
    }
}