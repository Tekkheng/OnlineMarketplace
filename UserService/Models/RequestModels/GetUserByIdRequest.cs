using MediatR;
using UserService.Models.ResponseModels;

namespace UserService.Models.RequestModels
{
    public class GetUserByIdRequest : IRequest<UserResponse?>
    {
        public string UserId { get; set; } = string.Empty;
    }
}
