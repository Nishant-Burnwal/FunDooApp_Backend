using System.ComponentModel.DataAnnotations;

namespace ModelLayer.DTOs.Labels
{
    public class UpdateLabelDTO
    {
        [Required]
        public string LabelName { get; set; }
    }
}
