# Especificação das classes do projeto

Este documento reúne a especificação de cada classe presente no projeto para servir como referência de contexto e evolução futura.

## Visão geral da arquitetura

O projeto segue uma arquitetura baseada em:
- um contexto central do personagem, responsável por expor dados e eventos importantes;
- um controlador de habilidades, que inicializa e executa todas as habilidades encontradas;
- habilidades específicas, cada uma com responsabilidade única;
- uma regra de extensão: novas habilidades devem ser adicionadas sem exigir alterações em habilidades existentes.

---

## Core

### AbilityController
- Responsabilidade: localizar todas as classes que herdam de Ability no GameObject e executá-las nos ciclos de vida do Unity.
- Comportamento:
  - no Awake, coleta todas as habilidades via GetComponents<Ability>();
  - chama Initialize() para cada uma;
  - no Update, executa Tick();
  - no FixedUpdate, executa FixedTick();
- Observação: atua como orquestrador do sistema de habilidades.

### CharacterContext
- Responsabilidade: servir como ponto central de acesso aos dados e serviços do personagem.
- Dados expostos:
  - Rigidbody2D do personagem;
  - CharacterMotor;
  - estado de chão via GroundDetector;
  - entrada de movimento e eventos de jump via PlayerInputReader.
- Métodos principais:
  - MoveInput: leitura da direção horizontal;
  - ConsumeJumpPressed(): consome o evento de jump pressionado;
  - ConsumeJumpReleased(): consome o evento de jump solto;
  - IsGrounded: indica se o personagem está no chão.
- Observação: reduz o acoplamento entre habilidades e componentes de baixo nível.

### CharacterMotor
- Responsabilidade: encapsular a lógica física básica do movimento do personagem.
- Funcionalidades:
  - definir velocidade vertical e horizontal;
  - ler velocidade vertical atual;
  - reduzir velocidade vertical para pulo variável;
  - controlar saltos com suporte a air jumps.
- Estado interno:
  - maxAirJumps: quantidade máxima de saltos extras;
  - remainingAirJumps: saltos extras ainda disponíveis;
  - wasGrounded: rastreia o último estado de contato com o chão.
- Observação: a lógica de coyote time e buffer de jump fica em JumpAbility, não nesta classe.

### GroundDetector
- Responsabilidade: identificar se o personagem está tocando o chão.
- Implementação:
  - usa Physics2D.OverlapCircle() em um ponto de verificação (groundCheck);
  - considera apenas a LayerMask definida para chão;
  - mantém um controle simples de estabilidade do estado de grounded.
- Observação: é um componente auxiliar para o contexto e para outras habilidades.

### PlayerInputReader
- Responsabilidade: capturar inputs do jogador via Unity Input System e expor eventos de forma simples para o restante do projeto.
- Funcionalidades:
  - lê o movimento do joystick ou teclado;
  - registra se o botão de pulo foi pressionado ou solto;
  - disponibiliza métodos para consumir esses eventos.
- Observação: centraliza a leitura de input para evitar espalhar dependências do Input System pelo projeto.

### CharacterState
- Responsabilidade: ainda não implementada; serve como ponto de extensão para futura modelagem de estados do personagem.
- Observação: atualmente está vazia e pode ser usada para representar estados como idle, running, jumping, falling, etc.

### PlayerInputActions
- Responsabilidade: classe gerada automaticamente pelo Unity Input System a partir do arquivo de definição de ações.
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
- Observação: funciona como habilidade de locomoção principal.

### JumpAbility
- Responsabilidade: implementar a lógica de pulo do personagem.
- Funcionalidades:
  - pulo comum;
  - coyote time;
  - jump buffer;
  - pulo variável ao soltar o botão.
- Comportamento:
  - atualiza contadores de tempo no Tick();
  - tenta executar o pulo quando houver buffer e coyote time válidos;
  - aplica redução de velocidade vertical ao soltar o botão.
- Observação: é uma habilidade independente e não depende de alteração em outras classes para funcionar.

### JumpDecorator
- Responsabilidade: classe de extensão planejada para a lógica de jump por decorator.
- Observação: atualmente vazia e pronta para futura implementação de padrões decoradores.

### DoubleJumpDecorator
- Responsabilidade: extensão futura para adicionar double jump sem alterar o comportamento base de JumpAbility.
- Observação: atualmente vazia.

### VariableJumpDecorator
- Responsabilidade: extensão futura para modularizar o comportamento de pulo variável.
- Observação: atualmente vazia.

### DashAbility
- Responsabilidade: futura habilidade de dash.
- Observação: arquivo criado como espaço para implementação futura.

### WallSlideAbility
- Responsabilidade: futura habilidade de wall slide.
- Observação: arquivo criado como espaço para implementação futura.

### WallJumpAbility
- Responsabilidade: futura habilidade de wall jump.
- Observação: arquivo criado como espaço para implementação futura.

---

## Regras de extensão do projeto

- Novas habilidades devem ser adicionadas como novas classes, sem exigir mudanças nas habilidades já existentes.
- O padrão esperado é que cada habilidade tenha uma responsabilidade única e isolada.
- O CharacterContext deve permanecer como o ponto de acesso comum a dados e eventos do personagem.
