Documentação Técnica de Game Design & Implementação: Sistema de Stamina (Estilo Celeste)
Público-alvo: Desenvolvedor de Gameplay / Programador de Jogos

Objetivo: Implementar um sistema de Stamina dinâmico, permissivo e visualmente claro para controlar o tempo de sustentação, escalada e ações especiais no jogo (como Wall Clings, Climb Jumps e Dashes/Abilidades), alinhado ao Game Feel e à fluidez de Celeste.

1. Visão Geral do Sistema
O sistema de Stamina serve como um recurso limitado que restringe o tempo continuo em ações de alta intensidade (como segurar em paredes ou realizar Climb Jumps). Este sistema deve ser tratado com o conceito de "Ability" já definida na nossa arquitetura e deve seguir todas as prerrogativas para o seu bom desempenho.

Princípios Fundamentais:

Recuperação Rápida: A stamina é completamente restaurada assim que o personagem toca o chão ou um elemento de recarga (ex: cristais/plataformas).

Custo Diferenciado: Ações estáticas consomem stamina gradualmente; ações ativas (como saltos de escalada) consomem blocos fixos e imediatos.

Feedback Transparente: O jogador deve saber exatamente seu estado de stamina através de mudanças visuais no personagem (cores, efeitos) e sonoras antes e durante a exaustão.

2. Parâmetros e Valores de Equilíbrio (Balancing)
Parâmetro	Valor Base Recomendado	Descrição
MaxStamina	110.0	Valor total da barra/reserva interna de stamina.
ClimbUpDrainRate	45.0 / seg	Taxa de consumo de stamina ao se mover para cima na parede.
ClimbStillDrainRate	10.0 / seg	Taxa de consumo de stamina ao ficar parado segurando a parede (Wall Grab).
ClimbDownDrainRate	0.0 / seg	Não consome stamina ao deslizar/descer a parede.
ClimbJumpCost	27.5	Custo fixo instantâneo por realizar um Climb Jump (pulo de escalada).
StaminaRecoveryDelay	0.1s	Tempo de espera após tocar o chão antes de iniciar a regeneração.
RefillOnGrounded	true	Restaura MaxStamina instantaneamente ao tocar o chão.
Nota: Com esses valores, o jogador consegue segurar na parede por aproximadamente 11 segundos estático, mover-se para cima por aproximadamente 2.4 segundos, ou realizar até 4 Climb Jumps antes de esgotar a stamina.

3. Estados do Sistema de Stamina
       [ NORMAL ]  ---> (Stamina <= 20) ---> [ LOW STAMINA / WARNING ]
           ^                                         |
           |                                         v
   (Toca no Chão) <------------------------- [ EXHAUSTED / STAMINA ZERO ]
3.1. Estado Normal (Stamina > 20)
O personagem executa todas as ações sem restrições.

Animações e cor da sprite padrão.

3.2. Estado Alerta / Pouca Stamina (0 < Stamina <= 20)
O jogador ainda pode realizar ações, mas recebe avisos visuais e sonoros de que a stamina está prestes a acabar.

Feedback:

Personagem começa a piscar em tom avermelhado ou emitir partículas de suor/fumaça.

Pitch do áudio de escalada aumenta ligeiramente.

3.3. Estado de Exaustão (Stamina <= 0)
O personagem solta a parede automaticamente e entra em queda livre/deslize não controlável.

Restrições:

Impossibilitado de usar Grab (segurar parede) ou realizar Climb Jump.

Permite apenas Wall Jump Normal (afastando-se da parede) ou Neutral Wall Jump.

Feedback:

Animação de fadiga/cansaço.

O personagem pisca continuamente em vermelho até pousar no chão.

4. Arquitetura e Estrutura de Código
4.1. Estrutura de Dados (Variáveis)
C#
public class StaminaController : MonoBehaviour
{
    [Header("Stamina Settings")]
    [SerializeField] private float maxStamina = 110f;
    [SerializeField] private float climbUpDrainRate = 45f;
    [SerializeField] private float climbStillDrainRate = 10f;
    [SerializeField] private float climbJumpCost = 27.5f;

    [Header("Warning Thresholds")]
    [SerializeField] private float lowStaminaThreshold = 20f;

    // Propriedades Públicas
    public float CurrentStamina { get; private set; }
    public bool IsExhausted => CurrentStamina <= 0;
    public bool IsLowStamina => CurrentStamina <= lowStaminaThreshold;

    private bool isGrounded;
    private bool isGrabbingWall;
    private float verticalInput;

    private void Awake()
    {
        CurrentStamina = maxStamina;
    }
}
4.2. Lógica de Atualização (Drenagem e Regeneração)
C#
private void Update()
{
    HandleStaminaDrain();
    HandleStaminaRefill();
    UpdateVisuals();
}

private void HandleStaminaDrain()
{
    if (isGrabbingWall && !isGrounded)
    {
        float drainRate = 0f;

        if (verticalInput > 0)
        {
            // Subindo a parede
            drainRate = climbUpDrainRate;
        }
        else if (verticalInput == 0)
        {
            // Segurando parado na parede
            drainRate = climbStillDrainRate;
        }

        CurrentStamina -= drainRate * Time.deltaTime;
        CurrentStamina = Mathf.Max(0, CurrentStamina);

        if (IsExhausted)
        {
            ForceReleaseWall();
        }
    }
}

private void HandleStaminaRefill()
{
    // Restaura a stamina instantaneamente ao tocar o chão
    if (isGrounded)
    {
        CurrentStamina = maxStamina;
    }
}

public bool TryConsumeStamina(float amount)
{
    if (CurrentStamina >= amount)
    {
        CurrentStamina -= amount;
        return true;
    }
    return false;
}
5. Integração com Outras Mecânicas
Wall Cling / Grab:

Antes de permitir que o jogador se fixe na parede, cheque if (!staminaController.IsExhausted && isGrabButtonPressed).

Climb Jump (Pulo de Escalada):

Ao pressionar o botão de pulo enquanto segura na parede:

C#
if (staminaController.CurrentStamina >= climbJumpCost)
{
    staminaController.TryConsumeStamina(climbJumpCost);
    PerformClimbJump();
}
else
{
    // Pulo neutro ou de afastamento sem custo extra, mas sem impulso vertical de escalada
    PerformNeutralWallJump();
}
Refills / Cristais de Dash/Stamina:

Coletáveis na fase podem chamar RefillStamina() instantaneamente no ar, permitindo sequências longas de puzzle de plataforma.

6. Feedback Visual e Sonoro (Juiciness & UX)
Para garantir excelente sensibilidade de jogo (Game Feel):

Sem Barra de UI Distrativa (Opcional): Em vez de uma barra fixa na tela, priorize feedback no próprio personagem (coloração da sprite, efeitos de suor/partículas).

Animação de Cansaço: Transição fluida de Climb Hold para Slide Fall quando a stamina atinge 0.

Efeitos Sonoros (SFX):

Som de suspiro do personagem ao entrar em exaustão.

Efeito de recarga sonora imediato ao tocar o chão.

7. Critérios de Aceite (Checklist para QA & Code Review)
[ ] Restauração no Chão: A stamina é completamente restaurada ao tocar qualquer superfície sólida/chão.

[ ] Drenagem Variável: Subir a parede consome stamina mais rápido do que ficar parado segurando-a.

[ ] Custo do Climb Jump: Executar um pulo direto para cima na parede subtrai o valor fixo da stamina imediatamente.

[ ] Desconexão Forçada: O jogador é descolado da parede no momento exato em que a stamina atinge zero.

[ ] Bloqueio de Re-Grab: Enquanto exausto e no ar, o jogador não consegue voltar a segurar em nenhuma parede.

[ ] Feedback de Pouca Stamina: É possível identificar visualmente que o personagem está ficando sem stamina antes que ela acabe.