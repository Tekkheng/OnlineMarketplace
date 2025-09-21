using MediatR;
using UserService.Models.ResponseModels;
using System.Collections.Generic;

namespace UserService.Models.RequestModels
{
    public class GetAllUsersRequest : IRequest<List<UserResponse>>
    {
 
    }
}
