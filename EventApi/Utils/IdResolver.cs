using Event.Model.Exception;
using HashidsNet;

namespace Event.Utils;

public interface IIdResolver
{
    public string Encrypt(int id);
    public int Decrypt(string id);
}

public class HashIdResolver : IIdResolver
{
    private readonly IHashids _hashids;

    public HashIdResolver(IHashids hashids)
    {
        _hashids = hashids;
    }

    public string Encrypt(int id)
    {
        var encoded = _hashids.Encode(id);
        if (encoded == null)
        {
            throw new Exception("Could not encode id");
        }

        return encoded;
    }

    public int Decrypt(string id)
    {
        var decoded = _hashids.Decode(id);
        if (decoded.Length != 1)
        {
            throw new NotFoundException($"Could not decode id {id}");
        }

        return decoded[0];
    }
}

public class DirectIdResolver : IIdResolver
{
    public string Encrypt(int id)
    {
        return $"{id}";
    }

    public int Decrypt(string id)
    {
        return int.Parse(id);
    }
}