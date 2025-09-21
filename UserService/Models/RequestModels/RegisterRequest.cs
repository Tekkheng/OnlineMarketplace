using MediatR;
using System.ComponentModel.DataAnnotations;
using UserService.Models.ResponseModels;

namespace UserService.Models.RequestModels;

public class RegisterRequest : IRequest<RegisterResponse>
{
    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;

    [Required]
    public string Role { get; set; } = string.Empty;
}