using System;

namespace ModelLayer.DTOs.Collaborators
{
    public class CollaboratorResponseDTO
    {
        public int CollaboratorId { get; set; }
        public int NoteId { get; set; }
        public string Email { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
