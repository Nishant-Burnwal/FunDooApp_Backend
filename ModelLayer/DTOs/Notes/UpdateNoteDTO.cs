using System;

namespace ModelLayer.DTOs.Notes
{
    public class UpdateNoteDTO
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public DateTime? Reminder { get; set; }
        public string? Colour { get; set; }
    }
}
