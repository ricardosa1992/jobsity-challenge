using JobsityChallenge.Core.DTOs;
using JobsityChallenge.Core.Interfaces.Services;
using JobsityChallenge.Core.Results;
using JobsityChallenge.Core.Utilities;
using Microsoft.AspNetCore.Identity;

namespace JobsityChallenge.Core.Services;

public class AuthService(UserManager<IdentityUser> userManager, JwtTokenGenerator jwtTokenGenerator) : IAuthService
{
    public Task<IdentityResult> RegisterAsync(RegisterUserDto registerDto)
    {
        var user = new IdentityUser
        {
            UserName = registerDto.UserName,
            Email = registerDto.Email
        };

        return userManager.CreateAsync(user, registerDto.Password);
    }

    public async Task<Result<string>> LoginAsync(LoginDto loginDto)
    {
        var user = await userManager.FindByNameAsync(loginDto.UserName);
        if (user == null || !await userManager.CheckPasswordAsync(user, loginDto.Password))
            return Errors.InvalidUserOrPassword;

        return jwtTokenGenerator.GenerateToken(user.Id, user.UserName!);
    }
}
