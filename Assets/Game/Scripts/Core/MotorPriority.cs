/// <summary>
/// Define a prioridade de aplicação das solicitações de movimento e gravidade no CharacterMotor.
/// Valores maiores sobrescrevem as solicitações de menor prioridade.
/// </summary>
public enum MotorPriority
{
    Movement = 0,

    WallJump = 50,

    Dash = 100,

    Knockback = 200
}