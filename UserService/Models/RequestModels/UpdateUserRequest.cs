using MediatR;
using UserService.Models.ResponseModels;

namespace UserService.Models.RequestModels
{
    public class UpdateUserRequest : IRequest<UserResponse>
    {
        public string UserId { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? NewPassword { get; set; }
        public string? Role { get; set; }
    }
}
