using MediatR;
using Microsoft.AspNetCore.Identity;
using UserService.Entities;
using UserService.Models.RequestModels;
using UserService.Models.ResponseModels;

namespace UserService.RequestHandlers
{
    public class DeleteUserHandler : IRequestHandler<DeleteUserRequest, UserResponse>
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public DeleteUserHandler(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<UserResponse> Handle(DeleteUserRequest request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.UserId);
            if (user == null)
            {
                return new UserResponse
                {
                    Message = "User not found"
                };
            }

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                return new UserResponse
                {
                    Message = "Failed to delete user"
                };
            }

            return new UserResponse
            {
                Id = user.Id,
                Email = user.Email ?? string.Empty,
                Role = string.Empty
            };
        }
    }
}
