namespace IDGenerator.Services.IDGenerator;

public interface IIDGeneratorService
{
    Task<string> Generate();
}