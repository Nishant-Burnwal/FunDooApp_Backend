using System.ComponentModel.DataAnnotations;

namespace ModelLayer.DTOs
{
    public class RegisterUserDTO
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        //[RegularExpression("")]
        public string Password { get; set; }
    }
}
