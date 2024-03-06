using System.ComponentModel.DataAnnotations;

namespace NZWalks.Api.Models.DTO
{
    public class AddRegionRequestDto
    {
        [Required]
        [MinLength(3,ErrorMessage ="Code should be minimum of 3 characters")]
        [MaxLength(3, ErrorMessage = "Code should be MAximum of 3 characters")]
        public string Code { get; set; }
        [Required]
        [MaxLength(100, ErrorMessage = "Code should be MAximum of 100 characters")]
        public string Name { get; set; }
        public string? RegionImageUrl { get; set; }
    }
}
