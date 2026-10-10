Documentação Técnica de Game Design & Implementação: Mechanics — Wall Jump
Público-alvo: Desenvolvedor Sênior de Jogos / Programador de Gameplay

Objetivo: Implementar uma mecânica de Wall Jump responsiva, permissiva e extremamente polida, seguindo as diretrizes de Game Feel de Celeste.

1. Visão Geral da Mecânica
O Wall Jump não se resume a checar se o jogador está encostado em uma parede e aplicar um impulso diagonal. Ele é um sistema flexível alimentado por múltiplos buffers, margens de tolerância (Coyote Time), vetores de força contextuais e controle de stamina/gravidade. Este sistema deve ser tratado com o conceito de "Ability" já definida na nossa arquitetura e deve seguir todas as prerrogativas para o seu bom desempenho.

O objetivo principal é garantir que a intenção do jogador sempre tenha prioridade sobre a precisão matemática dos colisores.

2. Parâmetros & Tolerâncias (Game Feel / Juiciness)
Para atingir a sensação de controle ideal, é necessário implementar os seguintes parâmetros de tolerância:

2.1. Wall Coyote Time
Descrição: Janela de tempo em que o jogador ainda pode executar um Wall Jump após ter se afastado da parede ou começado a cair.

Valor Recomendado: 0.08s a 0.12s (aprox. 5 a 7 frames a 60 FPS).

Comportamento: Guarde a referência da última parede tocada e a direção dela por esse período.

2.2. Input Buffering (Wall Jump Buffer)
Descrição: Se o jogador pressionar o botão de pulo ligeiramente antes de encostar na parede ou de entrar no estado válido, o pulo deve ser registrado e executado assim que a condição for satisfeita.

Valor Recomendado: 0.1s (6 frames a 60 FPS).

2.3. Corner Correction (Correção de Quina / Ledge Snap)
Descrição: Se o jogador tentar fazer um Wall Jump próximo ao topo de uma parede e seu personagem colidir com a quina/aresta superior (bloqueando a sobida), o jogo deve descolocar suavemente o personagem para cima para que ele supere o topo da plataforma em vez de "bater a cabeça".

Tolerância Vertical: 4 a 8 pixels.

3. Tipos de Pulo na Parede e Vetores de Força
Existem três variações principais de interações com a parede dependendo do input direcional no momento em que o botão de pulo é pressionado:

    [Wall Jump Normal]            [Neutral Wall Jump]             [Climb Jump]
 (Segurando p/ fora)            (Direção Neutra)           (Segurando p/ a parede)
       \                            |                            ^
        \                           |                            |
         v                          v                            |
  [Parede] |                  [Parede] |                  [Parede] |
3.1. Wall Jump Normal (Afastando-se da parede)
Condição: Pressionar o botão de Pulo segurando o analógico/D-Pad para longe da parede.

Vetor de Força: Força vertical moderada/alta + Força horizontal forte na direção oposta à parede.

Controle do Jogador: O jogador recupera o controle horizontal imediato após uma curtíssima janela de lockout (~0.05s).

3.2. Neutral Wall Jump (Salto Neutro)
Condição: Pressionar o botão de Pulo sem segurar nenhuma direção horizontal (ou segurando para a parede sem estar escalando).

Vetor de Força: Altura vertical maximizada (quase um pulo normal) com uma pequena repulsão horizontal para afastar o personagem da parede.

Uso: Permite ao jogador "escalar" a mesma parede usando pulos neutros intercalados com o retorno em direção à parede.

3.3. Climb Jump (Pulo de Escalada)
Condição: Pressionar o botão de Pulo enquanto o botão de Grab/Escalar está pressionado e a stamina é > 0.

Vetor de Força: Força estritamente vertical (sem repulsão horizontal significativa).

Custo: Consome um bloco significativo de Stamina (ex: 27.5% da barra total).

4. Gestão de Stamina e Deslizamento (Wall Slide / Grip)
Wall Slide (Deslizamento):

Quando encostado na parede caindo (Velocity.y < 0), aplique uma força de atrito que limite a velocidade máxima de queda (ex: Max Wall Slide Speed = 0.3 * Max Fall Speed).

Stamina System:

Capacidade Total: Suficiente para escalar continuamente por ~1.1 segundos.

Consumo:

Segurar na parede (estático): Consome stamina gradualmente a uma taxa baixa.

Mover-se para cima enquanto segura: Consome stamina a uma taxa maior.

Climb Jump (Pulo vertical segurando a parede): Desconta um valor fixo imediato.

Estado de Exaustão (Stamina Zero):

O personagem solta a parede, entra em animação de cansaço, pisca em vermelho e não pode realizar Climb Jumps nem Grab até tocar no chão novamente.

5. Máquina de Estados (FSM) & Lógica de Atualização
Transições de Estado
[InAir / Falling]  ---> (Detecta parede + Input) ---> [WallSlide / WallClimb]
[WallSlide / Grab] ---> (Pressiona Pulo)          ---> [Performing WallJump]
[Performing WallJump] ---> (Timer de Lockout expira) ---> [InAir / Controllable]
Pseudo-código da Lógica do Wall Jump:
C#
void ProcessWallJumpInputs()
{
    bool canWallJump = (IsTouchingWall() || wallCoyoteTimer > 0) && !IsGrounded();
    
    if (jumpBufferTimer > 0 && canWallJump)
    {
        // Reseta o buffer e o coyote
        jumpBufferTimer = 0;
        wallCoyoteTimer = 0;

        int wallDir = GetWallDirection(); // -1 para parede à esquerda, 1 para parede à direita
        int inputDir = GetHorizontalInput(); // -1, 0, 1

        if (isGrabbing && currentStamina > 0)
        {
            // 1. Climb Jump (Consome Stamina, impulso vertical puro)
            ApplyForce(new Vector2(0, climbJumpForce));
            ConsumeStamina(climbJumpStaminaCost);
        }
        else if (inputDir == -wallDir)
        {
            // 2. Wall Jump Normal (Impulso para longe da parede)
            ApplyForce(new Vector2(-wallDir * wallJumpHorizontalForce, wallJumpVerticalForce));
            SetControlLockout(lockoutDuration); // Bloqueia levemente o input oposto
        }
        else
        {
            // 3. Neutral Wall Jump (Impulso alto, pouca distância horizontal)
            ApplyForce(new Vector2(-wallDir * neutralJumpHorizontalForce, neutralJumpVerticalForce));
        }

        PlayAudio("event:/char/wall_jump");
        SpawnParticles(wallDir);
    }
}
6. Feedback Visual e Sonoro (Juice & Polish)
Para alinhar com a polidez o código deve disparar os seguintes eventos visuais/auditivos simultaneamente ao Wall Jump:

Squash & Stretch (Deformação da Malha/Sprite):

Ao pular da parede, achatar a largura da sprite (X = 0.7) e esticar a altura (Y = 1.3), retornando suavemente ao normal via interpolação (Lerp) em 0.15s.

Partículas (Dust / Sparkles):

Emitir uma explosão de partículas de poeira do ponto de contato do pé/mão com a parede na direção oposta ao pulo.

Screen Shake (Opcional/Ajustável):

Um micro-impacto na câmera (ex: 1-2 pixels de deslocamento por 0.05s) nos pulos de alta força.

Sombra / Ghost Trail:

Deixar uma rastro rápido de sprite semi-transparente caso o jogador faça um salto perfeito ou esteja em estado de alta velocidade.

## Decisões de escopo e integração do projeto

Esta seção registra as decisões atuais do projeto e prevalece sobre os trechos anteriores que descrevem Grab, Climb Jump, stamina e corner correction.

- A interação com paredes será baseada no `WallSlideAbility`. O jogo não terá uma ação de Grab.
- Climb Jump e seu vetor vertical dedicado não fazem parte do escopo atual. Os tipos de Wall Jump a considerar são o salto normal e o salto neutro; os critérios que dependem de Grab, Climb Jump ou stamina ficam adiados até a definição do sistema de stamina.
- A elegibilidade do salto é determinada pelo estado do personagem: durante `IsWallSliding`, o comando deve ser tratado como Wall Jump; fora desse estado, pode ser tratado como pulo normal.
- A implementação deve impedir que `JumpAbility` e `WallJumpAbility` consumam o mesmo `JumpPressed` no mesmo frame. Um Wall Jump executado também deve invalidar/consumir esse comando para impedir um segundo salto normal, inclusive quando o coyote time de chão ainda estiver ativo. A seleção não deve depender acidentalmente da ordem dos componentes no `AbilityController`.
- O Wall Jump terá seu próprio buffer de entrada e wall coyote time. Ao perder contato, deve conservar por toda a janela de coyote a direção da última parede válida; `WallDetector.WallDirection` zerado após a separação não pode ser usado como direção do impulso.
- Corner correction está fora do escopo desta implementação e poderá ser reavaliada em outra tarefa.
- Stamina e feedback específicos de stamina serão definidos em uma tarefa futura e acompanhados como débitos técnicos no documento `Stamina System Game Design.md`.

## Critérios de aceite revisados para o escopo atual

- [ ] Pulo normal e Wall Jump nunca são executados pelo mesmo comando de pulo.
- [ ] O Wall Jump pode ser executado durante o buffer e a janela de wall coyote configurados, usando a direção armazenada da última parede.
- [ ] Salto normal e salto neutro apresentam vetores distintos conforme a direção horizontal no momento do comando.
- [ ] Grab, Climb Jump, stamina e corner correction não são requisitos para concluir esta tarefa.

7. Critérios de Aceite (Checklist para QA & Code Review)
O desenvolvedor deve garantir que a mecânica atenda a 100% dos critérios abaixo antes de considerar a tarefa concluída:

[ ] Coyote Time Funcional: É possível realizar o Wall Jump até ~6 frames após sair do contato com a parede.

[ ] Input Buffer Funcional: Apertar o pulo ~6 frames antes de tocar a parede executa o pulo instantaneamente ao tocar nela.

[ ] Diferenciação de Inputs: Salto neutro, salto para longe e salto segurando escalada possuem vetores de força visivelmente distintos.

[ ] Corner Correction Ativo: O personagem não "tranca" a cabeça em quinas superiores ao tentar subir em paredes curtas via Wall Jump.

[ ] Gerenciamento de Stamina: A mecânica de exaustão impede escaladas infinitas e força o jogador a usar saltos neutros/estratégicos.

[ ] Sensação de Controle: Não há sensação de lag ou "personagem pesado" (sluggishness); o tempo de resposta é imediato.
