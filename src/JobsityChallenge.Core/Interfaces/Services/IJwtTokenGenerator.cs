namespace JobsityChallenge.Core.Interfaces.Services;

public interface IJwtTokenGenerator
{
    string GenerateToken(string userId, string userName);
}
