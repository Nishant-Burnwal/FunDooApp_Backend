using System.ComponentModel.DataAnnotations;

namespace ModelLayer.DTOs.Collaborators
{
    public class AddCollaboratorDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
