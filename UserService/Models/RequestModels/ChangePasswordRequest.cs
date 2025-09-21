using System.ComponentModel.DataAnnotations;
namespace UserService.Models.RequestModels;

public class ChangePasswordRequest
{
    [Required] public string NewPassword { get; set; } = string.Empty;
}