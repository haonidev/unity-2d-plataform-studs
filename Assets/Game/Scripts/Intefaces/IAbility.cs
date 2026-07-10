/// <summary>
/// Contrato mínimo para as habilidades do personagem no ciclo de vida do AbilityController.
/// </summary>
public interface IAbility
{
    /// <summary>
    /// Inicializa a habilidade.
    /// </summary>
    void Initialize();

    /// <summary>
    /// Executa a lógica principal da habilidade a cada frame.
    /// </summary>
    void Tick();

    /// <summary>
    /// Executa a lógica física da habilidade a cada atualização fixa.
    /// </summary>
    void FixedTick();
}