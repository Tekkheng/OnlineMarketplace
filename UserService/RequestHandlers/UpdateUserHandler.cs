using MediatR;
using Microsoft.AspNetCore.Identity;
using UserService.Entities;
using UserService.Models.RequestModels;
using UserService.Models.ResponseModels;

namespace UserService.RequestHandlers;
public class UpdateUserHandler : IRequestHandler<UpdateUserRequest, UserResponse>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public UpdateUserHandler(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task<UserResponse> Handle(UpdateUserRequest request, CancellationToken cancellationToken)
    {
        var response = new UserResponse();

        var user = await _userManager.FindByIdAsync(request.UserId);
        if (user == null)
        {
            response.Message = "User not found";
            return response;
        }

        if (!string.IsNullOrEmpty(request.Email))
        {
            user.Email = request.Email;
            user.UserName = request.Email;
        }

        var updateResult = await _userManager.UpdateAsync(user);
        if (!updateResult.Succeeded)
        {
            response.Message = "Failed to update user";
            return response;
        }

        if (!string.IsNullOrEmpty(request.NewPassword))
        {
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var passResult = await _userManager.ResetPasswordAsync(user, token, request.NewPassword);
            if (!passResult.Succeeded)
            {
                response.Message = "Failed to update password";
                return response;
            }
        }

        if (!string.IsNullOrEmpty(request.Role))
        {
            var currentRoles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, currentRoles);

            if (!await _roleManager.RoleExistsAsync(request.Role))
            {
                await _roleManager.CreateAsync(new IdentityRole(request.Role));
            }

            await _userManager.AddToRoleAsync(user, request.Role);
        }

        var roles = await _userManager.GetRolesAsync(user);
        response.Id = user.Id;
        response.Email = user.Email ?? string.Empty;
        response.Role = roles.FirstOrDefault() ?? string.Empty;
        response.Message = "User updated successfully";

        return response;
    }
}
