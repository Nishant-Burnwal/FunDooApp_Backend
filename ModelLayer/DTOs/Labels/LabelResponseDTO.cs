using System;

namespace ModelLayer.DTOs.Labels
{
    public class LabelResponseDTO
    {
        public int LabelId { get; set; }
        public string LabelName { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
