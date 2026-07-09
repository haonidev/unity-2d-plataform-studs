using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputReader : MonoBehaviour
{
    private PlayerInputActions inputActions;

    private readonly PlayerFrameInput frameInput = new();

    /// <summary>
    /// Estado do input referente ao frame atual.
    /// </summary>
    public PlayerFrameInput FrameInput => frameInput;

    private void Awake()
    {
        inputActions = new PlayerInputActions();
    }

    private void OnEnable()
    {
        inputActions.Enable();

        inputActions.Player.Move.performed += OnMove;
        inputActions.Player.Move.canceled += OnMove;

        inputActions.Player.Jump.performed += OnJump;
        inputActions.Player.Jump.canceled += OnJumpCanceled;
    }

    private void OnDisable()
    {
        inputActions.Player.Move.performed -= OnMove;
        inputActions.Player.Move.canceled -= OnMove;

        inputActions.Player.Jump.performed -= OnJump;
        inputActions.Player.Jump.canceled -= OnJumpCanceled;

        inputActions.Disable();
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        frameInput.Move = context.ReadValue<Vector2>();
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        frameInput.JumpPressed = true;
    }

    private void OnJumpCanceled(InputAction.CallbackContext context)
    {
        frameInput.JumpReleased = true;
    }

    /// <summary>
    /// Limpa todos os inputs de um único frame.
    /// Este método deve ser chamado apenas pelo AbilityController.
    /// </summary>
    public void ClearFrameInputs()
    {
        //TODO: rever isso!
        //JumpPressed = false;
        //JumpReleased = false;
    }
}