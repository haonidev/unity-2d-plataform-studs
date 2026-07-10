using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Traduz os inputs do Input System para sinais simples consumidos pelo CharacterContext e pelas habilidades.
/// </summary>
public class PlayerInputReader : MonoBehaviour
{
    private PlayerInputActions inputActions;

    private readonly PlayerFrameInput frameInput = new();

    /// <summary>
    /// Estado do input referente ao frame atual.
    /// </summary>
    public PlayerFrameInput FrameInput => frameInput;

    /// <summary>
    /// Cria a instância das ações de input no Awake.
    /// </summary>
    private void Awake()
    {
        inputActions = new PlayerInputActions();
    }

    /// <summary>
    /// Registra os callbacks dos inputs quando o componente entra em cena.
    /// </summary>
    private void OnEnable()
    {
        inputActions.Enable();

        inputActions.Player.Move.performed += OnMove;
        inputActions.Player.Move.canceled += OnMove;

        inputActions.Player.Jump.performed += OnJump;
        inputActions.Player.Jump.canceled += OnJumpCanceled;
        inputActions.Player.Dash.performed += OnDash;
    }

    /// <summary>
    /// Remove os callbacks dos inputs quando o componente sai de cena.
    /// </summary>
    private void OnDisable()
    {
        inputActions.Player.Move.performed -= OnMove;
        inputActions.Player.Move.canceled -= OnMove;

        inputActions.Player.Jump.performed -= OnJump;
        inputActions.Player.Jump.canceled -= OnJumpCanceled;
        inputActions.Player.Dash.performed -= OnDash;

        inputActions.Disable();
    }

    /// <summary>
    /// Atualiza a entrada de movimento com o valor lido no input action.
    /// </summary>
    private void OnMove(InputAction.CallbackContext context)
    {
        frameInput.Move = context.ReadValue<Vector2>();
    }

    /// <summary>
    /// Marca que o botão de pulo foi pressionado neste frame.
    /// </summary>
    private void OnJump(InputAction.CallbackContext context)
    {
        frameInput.JumpPressed = true;
    }

    /// <summary>
    /// Marca que o botão de pulo foi liberado neste frame.
    /// </summary>
    private void OnJumpCanceled(InputAction.CallbackContext context)
    {
        frameInput.JumpReleased = true;
    }

    /// <summary>
    /// Marca que o botão de dash foi pressionado neste frame.
    /// </summary>
    private void OnDash(InputAction.CallbackContext context)
    {
        frameInput.DashPressed = true;
    }

    /// <summary>
    /// Limpa os inputs transitórios do frame atual.
    /// TODO: este método ainda precisa ser consolidado com o fluxo de consumo do frame para evitar estados residuais.
    /// </summary>
    public void ClearFrameInputs()
    {
    }
}