using System;

namespace ModelLayer.DTOs.Notes
{
    public class CreateNoteDTO
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public DateTime? Reminder { get; set; }
        public string Colour { get; set; } = "#FFFFFF";
        public string? Image { get; set; }
    }
}
