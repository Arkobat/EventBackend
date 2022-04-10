using Microsoft.Extensions.DependencyInjection;

namespace EventTests;

public abstract class TestBase
{
    protected ServiceCollection Service { get; } = new();
    protected ServiceProvider ServiceProvider => Service.BuildServiceProvider();
}