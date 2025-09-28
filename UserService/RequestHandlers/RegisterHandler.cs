using MediatR;
using Microsoft.AspNetCore.Identity;
using UserService.Entities;
using UserService.Models.RequestModels;
using UserService.Models.ResponseModels;

namespace UserService.RequestHandlers;

public class RegisterHandler : IRequestHandler<RegisterRequest, RegisterResponse>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public RegisterHandler(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task<RegisterResponse> Handle(RegisterRequest request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Email))
            return new RegisterResponse { Message = "Email is required." };

        if (string.IsNullOrWhiteSpace(request.Password) || request.Password.Length < 6)
            return new RegisterResponse { Message = "Password must be at least 6 characters long." };

        if (string.IsNullOrWhiteSpace(request.Role))
            return new RegisterResponse { Message = "Role is required." };

        var existingUser = await _userManager.FindByEmailAsync(request.Email);
        if (existingUser != null)
            return new RegisterResponse { Message = "User with this email already exists." };

        var user = new ApplicationUser
        {
            UserName = request.Email,
            Email = request.Email,
            SecurityStamp = Guid.NewGuid().ToString()
        };

        var result = await _userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
            return new RegisterResponse { Message = string.Join(", ", result.Errors.Select(e => e.Description)) };
        
        var allowedRoles = new List<string> { "staff", "admin" };
        if (!allowedRoles.Contains(request.Role.ToLower()))
        {
            return new RegisterResponse { Message = "Invalid role. Only 'staff' or 'admin' are allowed." };
        }

        if (!await _roleManager.RoleExistsAsync(request.Role))
            await _roleManager.CreateAsync(new IdentityRole(request.Role));

        await _userManager.AddToRoleAsync(user, request.Role);

        return new RegisterResponse { Message = "User registered successfully!" };
    }
}
