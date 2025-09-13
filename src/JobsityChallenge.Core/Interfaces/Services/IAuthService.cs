using JobsityChallenge.Core.DTOs;
using JobsityChallenge.Core.Results;
using Microsoft.AspNetCore.Identity;

namespace JobsityChallenge.Core.Interfaces.Services;

public interface IAuthService
{
    Task<IdentityResult> RegisterAsync(RegisterUserDto registerUserDto);
    Task<Result<string>> LoginAsync(LoginDto loginDto);
}