using System.ComponentModel.DataAnnotations;

namespace ModelLayer.DTOs.User
{
    public class ForgotPasswordDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
