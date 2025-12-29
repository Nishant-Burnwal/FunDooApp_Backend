using System;

namespace ModelLayer.DTOs.Notes
{
    public class NoteResponseDTO
    {
        public int NotesId { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }

        public bool IsPin { get; set; }
        public bool IsArchive { get; set; }
        public bool IsTrash { get; set; }

        public string Colour { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
