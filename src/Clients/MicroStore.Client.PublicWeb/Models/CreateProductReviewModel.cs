using System.ComponentModel.DataAnnotations;

namespace MicroStore.Client.PublicWeb.Models
{
    public class CreateProductReviewModel
    {
        [Range(1, 5)]
        public int Rating { get; set; }

        [Required]
        [MaxLength(265)]
        [MinLength(3)]
        public string ReviewTitle { get; set; }

        [Required]
        [MaxLength(265)]
        [MinLength(3)]
        public string ReviewText { get; set; }
    }
}
