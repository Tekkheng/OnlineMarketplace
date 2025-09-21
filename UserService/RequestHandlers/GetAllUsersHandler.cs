using MediatR;
using Microsoft.AspNetCore.Identity;
using UserService.Entities;
using UserService.Models.RequestModels;
using UserService.Models.ResponseModels;

namespace UserService.RequestHandlers
{
    public class GetAllUsersHandler : IRequestHandler<GetAllUsersRequest, List<UserResponse>>
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public GetAllUsersHandler(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<List<UserResponse>> Handle(GetAllUsersRequest request, CancellationToken cancellationToken)
        {
            var users = _userManager.Users.ToList();

            if (users == null || users.Count == 0)
            {
                return new List<UserResponse>
                {
                    new UserResponse { Message = "No data available." }
                };
            }

            var userResponses = new List<UserResponse>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                userResponses.Add(new UserResponse
                {
                    Id = user.Id,
                    Email = user.Email ?? string.Empty,
                    Role = roles.FirstOrDefault() ?? string.Empty
                });
            }

            return userResponses;
        }
    }
}