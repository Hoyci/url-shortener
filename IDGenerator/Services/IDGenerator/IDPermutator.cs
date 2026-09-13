using System.Security.Cryptography;

namespace IDGenerator.Services.IDGenerator;

public class IDPermutator
{
    private readonly uint _key;

    public IDPermutator(uint key)
    {
        _key = key;
    }

    public uint Encrypt(uint value)
    {
        return Feistel(value, false);
    }

    public uint Decrypt(uint value)
    {
        return Feistel(value, true);
    }

    private uint Feistel(uint value, bool decrypt)
    {
        ushort left = (ushort)(value >> 16);
        ushort right = (ushort)(value & 0xFFFF);

        for (int i = 0; i < 8; i++)
        {
            int round = decrypt ? 7 - i : i;

            ushort temp = right;

            right = (ushort)(left ^ RoundFunction(right, round));

            left = temp;
        }

        return ((uint)left << 16) | right;
    }

    private ushort RoundFunction(ushort value, int round)
    {
        uint x = value;

        x ^= _key;
        x += (uint)(round * 0x9E3779B9);

        x ^= x >> 16;
        x *= 0x85EBCA6B;
        x ^= x >> 13;

        return (ushort)(x & 0xFFFF);
    }
}