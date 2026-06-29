


# Arquitetura do projeto

## Estrutura atual das pastas

```text
Assets/Game/Scripts/
├── Abilities/
│   ├── Ability.cs
│   ├── Jump/
│   │   ├── JumpAbility.cs
│   │   ├── JumpDecorator.cs
│   │   ├── DoubleJumpDecorator.cs
│   │   └── VariableJumpDecorator.cs
│   ├── Movement/
│   │   └── MoveAbility.cs
│   ├── Dash/
│   │   └── DashAbility.cs
│   └── Wall/
│       ├── WallSlideAbility.cs
│       └── WallJumpAbility.cs
├── Core/
│   ├── AbilityController.cs
│   ├── CharacterContext.cs
│   ├── CharacterMotor.cs
│   ├── CharacterState.cs
│   ├── GroundDetector.cs
│   ├── PlayerInputActions.cs
│   └── PlayerInputReader.cs
├── Components/
├── Data/
├── Intefaces/
│   └── IAbility.cs
└── Specs/
    ├── achitecture.md
    └── classes-specs.md
```

## Visão geral

A arquitetura é baseada em quatro blocos principais:

- Core: componentes centrais do personagem e orquestração do fluxo de gameplay.
- Abilities: habilidades específicas com responsabilidade única.
- Contexto: CharacterContext atua como ponto de acesso comum para dados e eventos.
- Extensão: novas habilidades devem ser adicionadas como novas classes, sem obrigar mudanças em habilidades existentes.

## Regras de design

1. Nenhuma habilidade nova deve exigir alteração em outra habilidade já existente.
2. Toda habilidade nova deve herdar de Ability ou, quando necessário, ampliar a lógica por meio de decoradores.
3. O acesso a dados do personagem deve passar por CharacterContext.
4. Lógica física básica deve ficar em CharacterMotor e GroundDetector.
5. Inputs devem ser lidos em PlayerInputReader e consumidos pelo contexto.

## Responsabilidades por camada

### Core
- AbilityController: encontra todas as habilidades no GameObject e as executa nos ciclos de vida do Unity.
- CharacterContext: centraliza acesso a Rigidbody2D, CharacterMotor, GroundDetector e input do jogador.
- CharacterMotor: encapsula movimentação física básica, velocidade horizontal/vertical e transições simples de estado.
- GroundDetector: verifica contato com o chão usando um overlap circle e uma camada específica.
- PlayerInputReader: traduz inputs do Input System para eventos simples consumíveis.
- CharacterState: espaço para evolução futura de estados de personagem.

### Abilities
- Ability: classe base para o ciclo de vida das habilidades.
- MoveAbility: controla movimento horizontal.
- JumpAbility: controla pulo, coyote time e jump buffer.
- JumpDecorator: base para extensões do comportamento de pulo.
- DoubleJumpDecorator: adiciona salto extra sem mexer na lógica base de JumpAbility.
- DashAbility, WallSlideAbility e WallJumpAbility: espaços para futuras habilidades.

## Passo a passo genérico para implementar uma nova habilidade

### 1. Definir a responsabilidade
- A nova habilidade deve ter uma única função clara, como dash, wall slide, wall jump, stun, slow time ou ataque.
- Ela não deve assumir responsabilidades de outra habilidade já existente.

### 2. Criar a classe da habilidade
- Criar um novo arquivo em uma pasta apropriada dentro de Abilities.
- A classe deve herdar de Ability.
- Implementar Initialize(), Tick() e/ou FixedTick() conforme o comportamento necessário.

### 3. Usar o contexto, não componentes diretamente
- A habilidade deve acessar o personagem por meio de CharacterContext.
- Exemplo: usar Context.MoveInput, Context.IsGrounded, Context.Motor e Context.ConsumeJumpPressed().
- Restrição: não invocar diretamente o Rigidbody2D ou o Input System na habilidade.

### 4. Respeitar o ciclo de vida
- O AbilityController já chama Initialize() no Awake, Tick() no Update e FixedTick() no FixedUpdate.
- A habilidade não precisa registrar si mesma manualmente.
- Restrição: não modificar o AbilityController para cada nova habilidade.

### 5. Isolar efeitos físicos e estado
- Movimento, salto e detecção de chão permanecem encapsulados em CharacterMotor e GroundDetector.
- Se a nova habilidade precisar de estado próprio, ele deve ficar dentro da própria classe.
- Restrição: não espalhar lógica de estado por outras habilidades.

### 6. Considerar extensibilidade
- Se a nova habilidade for uma variação de uma existente, prefira composição ou decoradores em vez de alterar a habilidade base.
- Restrição: não alterar o código de JumpAbility para adicionar Double Jump, Dash ou Wall Slide.

### 7. Validar e testar no cenário
- Confirmar que a habilidade funciona com o personagem já existente.
- Garantir que não interrompe movimento, pulo ou detecção de chão.

## Responsabilidades e restrições das classes envolvidas

### Ability
- Responsabilidade: fornecer o contrato base para o ciclo de vida das habilidades.
- Restrição: não deve conter lógica específica de gameplay; apenas o padrão comum.

### AbilityController
- Responsabilidade: localizar e executar as habilidades encontradas no GameObject.
- Restrição: não deve conhecer detalhes de cada habilidade; apenas orquestrar.

### CharacterContext
- Responsabilidade: expor dados e eventos do personagem de forma centralizada.
- Restrição: não deve guardar lógica de habilidade específica.

### CharacterMotor
- Responsabilidade: aplicar e consultar movimento físico básico.
- Restrição: não deve tratar lógica de input, buffers ou estados de habilidade específicos.

### GroundDetector
- Responsabilidade: informar se o personagem está no chão.
- Restrição: não deve decidir regras de gameplay; apenas detectar contato.

### PlayerInputReader
- Responsabilidade: converter input do Input System em sinais simples.
- Restrição: não deve conter lógica de movimentação ou habilidade.

### JumpDecorator (quando aplicável)
- Responsabilidade: estender o comportamento de pulo sem alterar JumpAbility.
- Restrição: deve ser uma extensão isolada e baseada no contrato do decorador.

## Resumo da regra de extensão

A arquitetura foi pensada para permitir crescimento sem acoplamento excessivo. Cada nova habilidade deve ser adicionada como uma nova classe, mantendo sua responsabilidade local e acessando o restante do sistema por meio de CharacterContext e das APIs já existentes.