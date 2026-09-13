using System.Text;
using IDGenerator.Repositories.Redis;

namespace IDGenerator.Services.IDGenerator;

public class IDGeneratorService : IIDGeneratorService
{
    private const string Alphabet = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
    private readonly IRepositoryRedis _redis;
    private readonly IDPermutator _permutator;

    public IDGeneratorService(
        IRepositoryRedis redis,
        IDPermutator permutator
    )
    {
        _redis = redis;
        _permutator = permutator;
    }

    public async Task<string> Generate()
    {
        var counter = await _redis.GetCount();
        var permuted = _permutator.Encrypt((uint)counter);
        return Encode(permuted);
    }

    private string Encode(uint value)
    {
        if (value == 0)
            return Alphabet[0].ToString();

        var builder = new StringBuilder();

        while (value > 0)
        {
            builder.Insert(
                0,
                Alphabet[(int)(value % Alphabet.Length)]
            );

            value /= (uint)Alphabet.Length;
        }

        return builder.ToString();
    }
}