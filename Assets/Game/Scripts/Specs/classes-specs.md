# Especificação das classes do projeto

Este documento reúne a especificação atual das classes do projeto para servir como referência de contexto, evolução futura e padronização de implementação.

## Visão geral da arquitetura

O projeto segue uma arquitetura baseada em:
- um contexto central do personagem, responsável por expor dados e eventos importantes;
- um controlador de habilidades, que inicializa e executa todas as habilidades encontradas;
- habilidades específicas, cada uma com responsabilidade única;
- um estado centralizado do personagem para animação, áudio e feedback;
- uma regra de extensão: novas habilidades e sistemas devem ser adicionados sem exigir alterações em módulos já existentes.

---

## Core

### AbilityController
- Responsabilidade: localizar todas as classes que herdam de Ability no GameObject e executá-las nos ciclos de vida do Unity.
- Comportamento:
  - no Awake, coleta todas as habilidades via GetComponents<Ability>();
  - chama Initialize() para cada habilidade;
  - no Update, executa Tick();
  - no FixedUpdate, executa FixedTick();
- Observação: atua como orquestrador do sistema de habilidades e não deve ser alterado para adicionar novas habilidades.

### CharacterContext
- Responsabilidade: servir como ponto central de acesso aos dados e serviços do personagem.
- Dados expostos:
  - Rigidbody2D do personagem;
  - CharacterMotor;
  - GroundDetector;
  - WallDetector;
  - PlayerInputReader;
  - CharacterState;
  - entrada de movimento e eventos de pulo via PlayerFrameInput.
- Métodos principais:
  - FrameInput: leitura do estado do frame atual;
  - MoveInput: leitura da direção horizontal;
  - JumpPressed: indica pulo pressionado;
  - JumpReleased: indica pulo liberado;
  - DashPressed: indica dash pressionado;
  - IsGrounded: indica se o personagem está no chão;
  - IsTouchingWall: indica contato com parede;
  - WallDirection: direção da parede detectada.
- Observação: reduz o acoplamento entre habilidades e componentes de baixo nível.

### CharacterMotor
- Responsabilidade: encapsular a lógica física básica do movimento do personagem.
- Funcionalidades:
  - definir velocidade vertical e horizontal;
  - ler velocidade vertical atual;
  - reduzir velocidade vertical para pulo variável;
  - aplicar prioridades de movimento e gravidade;
  - bloquear temporariamente o controle horizontal.
- Observação: a lógica de coyote time e buffer de jump fica em JumpAbility, não nesta classe.

### GroundDetector
- Responsabilidade: identificar se o personagem está tocando o chão.
- Implementação:
  - usa Physics2D.OverlapCircle() em um ponto de verificação (groundCheck);
  - considera apenas a LayerMask definida para chão;
  - mantém um controle simples de estabilidade do estado de grounded.
- Observação: é um componente auxiliar para o contexto e para outras habilidades.

### WallDetector
- Responsabilidade: identificar contato com paredes laterais.
- Implementação:
  - usa overlap circle em pontos de verificação à esquerda e à direita;
  - produz um valor de direção para habilitar wall slide e wall jump;
  - expõe o estado de contato pelo CharacterContext.

### PlayerInputReader
- Responsabilidade: capturar inputs do jogador via Unity Input System e expor eventos simples para o restante do projeto.
- Funcionalidades:
  - lê o movimento do joystick ou teclado;
  - registra se o botão de pulo foi pressionado ou solto;
  - registra se o botão de dash foi pressionado;
  - disponibiliza os valores via PlayerFrameInput.
- Observação: centraliza a leitura de input para evitar espalhar dependências do Input System pelo projeto.

### PlayerFrameInput
- Responsabilidade: representar as entradas do jogador durante um único frame.
- Observação: entradas transitórias como JumpPressed, JumpReleased e DashPressed são limpas ao fim do ciclo de Update.

### CharacterState
- Responsabilidade: armazenar o estado atual do personagem e publicar eventos usados por animação, áudio e VFX.
- Observação: pode ser usada para representar estados como idle, running, jumping, falling, attack, dash, wall slide, etc.

### PlayerInputActions
- Responsabilidade: classe gerada automaticamente pelo Unity Input System a partir do asset de input.
- Observação: não deve ser editada manualmente; alterações devem ser feitas no asset de input e regeneradas.

---

## Abilities

### Ability
- Responsabilidade: classe base para todas as habilidades do personagem.
- Funcionalidades:
  - armazena uma referência protegida a CharacterContext;
  - inicializa o contexto automaticamente via Initialize();
  - define os pontos de extensão Tick() e FixedTick();
- Observação: toda habilidade nova deve herdar desta classe para integrar-se ao sistema do AbilityController.

### IAbility
- Responsabilidade: interface mínima para garantir que todas as habilidades implementem o ciclo básico do sistema.
- Métodos:
  - Initialize();
  - Tick();
  - FixedTick();

### MoveAbility
- Responsabilidade: aplicar movimento horizontal do personagem.
- Comportamento:
  - lê a entrada horizontal de Context.MoveInput;
  - calcula velocidade alvo com base em moveSpeed;
  - aplica aceleração/desaceleração;
  - reduz a aceleração no ar com airControlMultiplier;
  - envia a velocidade resultante para o CharacterMotor.
- Observação: funciona como habilidade de locomoção principal e é um bom modelo para outras habilidades de movimento.

### JumpAbility
- Responsabilidade: implementar a lógica de pulo do personagem.
- Funcionalidades:
  - pulo comum;
  - coyote time;
  - jump buffer;
  - integração com decorators para variações de pulo.
- Comportamento:
  - atualiza contadores de tempo no Tick();
  - tenta executar o pulo quando houver buffer e coyote time válidos;
  - notifica decorators antes e depois da execução do salto.
- Observação: é uma habilidade independente e não deve ser alterada para adicionar novas variações de pulo.

### JumpDecorator
- Responsabilidade: base comum para extensões do comportamento de pulo, usando o padrão decorator.
- Observação: é o ponto de extensão recomendado para adicionar comportamentos como double jump, pulo variável ou modificações de altura sem mexer em JumpAbility.

### DoubleJumpDecorator
- Responsabilidade: extensão para adicionar double jump sem alterar o comportamento base de JumpAbility.
- Observação: funciona por meio de um contador de saltos usados e reinicialização ao tocar o chão.

### VariableJumpDecorator
- Responsabilidade: modularizar o comportamento de pulo variável.
- Observação: permanece isolada da lógica principal do pulo.

### DashAbility
- Responsabilidade: controlar o dash do personagem com duração, cooldown e estado de execução.
- Observação: o movimento do dash é aplicado pelo CharacterMotor com prioridade específica.

### WallSlideAbility
- Responsabilidade: habilitar o wall slide quando o personagem está encostado em uma parede e caindo.
- Observação: altera o estado de wall slide e limita a velocidade de queda.

### WallJumpAbility
- Responsabilidade: executar o wall jump quando o personagem está em wall slide e pressiona o botão de salto.
- Observação: aplica impulso horizontal e vertical e encerra o estado de wall slide.

---

## Regras de extensão do projeto

- Novas habilidades devem ser adicionadas como novas classes, sem exigir mudanças nas habilidades já existentes.
- O padrão esperado é que cada habilidade tenha uma responsabilidade única e isolada.
- O CharacterContext deve permanecer como o ponto de acesso comum a dados e eventos do personagem.
- Novas funcionalidades que alterem a lógica de uma habilidade base devem preferir decoradores ou classes complementares em vez de modificar a implementação principal.
- O AbilityController deve continuar funcionando sem necessidade de registro manual por habilidade.
- Sistemas como itens, inimigos e traps devem seguir este mesmo princípio: um componente por responsabilidade, com comunicação por eventos ou estado compartilhado.
