


Scripts
│
├── Core
│   ├── CharacterMotor.cs
│   ├── CharacterContext.cs
│   ├── GroundDetector.cs
│   ├── AbilityController.cs
│   ├── PlayerinputActions.cs
│   ├── PlayerinputReader.cs
│   └── CharacterState.cs
│
│
├── Components
│
│
├── Data
│
│
├── Interfaces
│   └── IAbility.cs
│
│
└── Abilities
    ├── Ability.cs
    │
    ├── Movement
    │    └── MoveAbility.cs
    │
    ├── Jump
    │    ├── JumpAbility.cs
    │    ├── JumpDecorator.cs
    │    ├── DoubleJumpDecorator.cs
    │    └── VariableJumpDecorator.cs [Vazio]
    │
    ├── Dash
    │    └── DashAbility.cs [Vazio]
    │
    └── Wall
         ├── WallSlideAbility.cs [Vazio]
         └── WallJumpAbility.cs [Vazio]


Regra do projeto: nenhuma nova habilidade (Dash, Double Jump, Wall Slide, etc.) deverá exigir alteração no código de outra habilidade já existente. Apenas adicionar novas classes.