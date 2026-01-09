using System.ComponentModel.DataAnnotations;

namespace ModelLayer.DTOs.Labels
{
    public class CreateLabelDTO
    {
        [Required]
        public string LabelName { get; set; }
    }
}
