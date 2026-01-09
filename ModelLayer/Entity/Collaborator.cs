using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModelLayer.Entity
{
    [Table("Collaborators")]
    public class Collaborator
    {
        [Key]
        public int CollaboratorId { get; set; }

        [Required]
        public int NoteId { get; set; }

        [Required]
        public int OwnerUserId { get; set; }

        [Required]
        [EmailAddress]
        public string CollaboratorEmail { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
