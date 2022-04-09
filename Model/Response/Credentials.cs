namespace Event.Model.Response;

public class Credentials
{
    public string AccessToken { get; init; } = null!;
    public DateTimeOffset AccessExpire { get; init; }

    public string RefreshToken { get; init; } = null!;
    public DateTimeOffset RefreshExpire { get; init; }
}