using System.ComponentModel.DataAnnotations;

namespace ModelLayer.DTOs.User
{
    public class ResetPasswordDTO
    {
        [Required]
        public string Token { get; set; }

        [Required]
        [MinLength(5)]
        public string NewPassword { get; set; }
    }
}
