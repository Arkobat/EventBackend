using System.Collections.Generic;
using Event.Utils;
using HashidsNet;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace EventTests.Utils;

public class HashIdResolverTest: TestBase
{
    private readonly HashIdResolver _resolver;

    public HashIdResolverTest()
    {
        Service.AddScoped<HashIdResolver>();
        Service.AddScoped<IHashids>(_ => new Hashids("token", 3));

        _resolver = ServiceProvider.GetRequiredService<HashIdResolver>();
    }

    [Test]
    public void ValidateIdsAreDifferent()
    {
        var ids = new HashSet<string>();
        for (var i = 0; i < 1000; i += 9)
        {
            var id = _resolver.Encrypt(i);
            Assert.IsTrue(ids.Add(id));
        }
    }

    [Test]
    public void ValidateDecryption()
    {
        for (var i = 0; i < 10; i++)
        {
            var encrypted = _resolver.Encrypt(i);
            var decrypted = _resolver.Decrypt(encrypted);
            Assert.That(i, Is.EqualTo(decrypted));
        }
    }
    
}