using MediatR;
using Microsoft.AspNetCore.Identity;
using UserService.Entities;
using UserService.Models.RequestModels;
using UserService.Models.ResponseModels;

namespace UserService.RequestHandlers;

public class GetUserByIdHandler : IRequestHandler<GetUserByIdRequest, UserResponse?>
{
    private readonly UserManager<ApplicationUser> _userManager;

    public GetUserByIdHandler(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<UserResponse?> Handle(GetUserByIdRequest request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.UserId);
        if (user == null) return null;

        var roles = await _userManager.GetRolesAsync(user);

        return new UserResponse
        {
            Id = user.Id,
            Email = user.Email,
            Role = roles.FirstOrDefault() ?? string.Empty
        };
    }
}
