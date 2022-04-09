using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Event.Database;
using Event.Model;
using Event.Model.Exception;
using Event.Model.Request;
using Event.Model.Response;
using Event.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Event.Service;

public class AuthService
{
    private readonly IConfiguration _config;
    private readonly EventDbContext _dbContext;
    private readonly IPasswordHasher _passwordHasher;

    public AuthService(IConfiguration config, EventDbContext dbContext, IPasswordHasher passwordHasher)
    {
        _config = config;
        _dbContext = dbContext;
        _passwordHasher = passwordHasher;
    }

    public async Task<Credentials> Login(LoginTo request)
    {
        var user = await _dbContext.Users
            .Where(u => u.Username == request.Username)
            .FirstOrDefaultAsync();

        if (user != null)
        {
            var hashedPassword = _passwordHasher.HashPassword(request.Password, user.Salt);
            if (user.Password != hashedPassword)
            {
                // Set user to null, ensuring the error is
                // always the same for wrong username and password
                user = null;
            }
        }

        if (user == null)
        {
            throw new BadRequestException("Invalid login information");
        }

        return GenerateTokens(user);
    }

    public async Task CreateUser(CreateUserTo to)
    {
        var salt = _passwordHasher.GenerateSalt();
        var user = new User
        {
            Username = to.Username.ToLower(),
            Email = to.Email.ToLower(),
            Password = _passwordHasher.HashPassword(to.Password, salt),
            Salt = salt
        };

        try
        {
            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            throw new BadRequestException("User already registered");
        }
    }

    public async Task<Credentials> RefreshToken(int memberId)
    {
        var user = await _dbContext.Users
            .Where(u => u.Id == memberId)
            .FirstOrDefaultAsync();

        if (user == null)
        {
            throw new NotFoundException("Could not find user");
        }

        return GenerateTokens(user);
    }

    private Credentials GenerateTokens(User user)
    {
        var accessToken = GenerateToken(user, TokenType.Access, TimeSpan.FromHours(6));
        var refreshToken = GenerateToken(user, TokenType.Refresh, TimeSpan.FromDays(90));

        return new Credentials
        {
            AccessToken = accessToken,
            AccessExpire = DateTimeOffset.Now + TimeSpan.FromHours(6),
            RefreshToken = refreshToken,
            RefreshExpire = DateTimeOffset.Now + TimeSpan.FromDays(90)
        };
    }

    private string GenerateToken(User user, TokenType tokenType, TimeSpan lifeSpan)
    {
        var config = _config.GetSection("Jwt");
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.GetValue<string>("Key")));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512);

        var memberIdKey = tokenType switch
        {
            TokenType.Access => "MemberId",
            TokenType.Refresh => "Refresh/MemberId",
            _ => throw new ArgumentException("Invalid TokenType")
        };

        var claims = new[]
        {
            new Claim(memberIdKey, user.Id.ToString()),
            new Claim(ClaimTypes.NameIdentifier, user.Username),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, "User")
        };

        var token = new JwtSecurityToken(
            config.GetValue<string>("Issuer"),
            config.GetValue<string>("Audience"),
            claims,
            expires: DateTime.Now + lifeSpan,
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private enum TokenType
    {
        Access,
        Refresh
    }
}