using System.ComponentModel.DataAnnotations;

namespace INWalksAPI.Models.DTO
{
    public class AddRegionRequestDto
    {
        [Required]
        [MinLength(3, ErrorMessage ="Region code Must Contains minimum 3 characters")]
        [MaxLength(4, ErrorMessage = "Region code allows up to 4 characters only")]
        public string Code { get; set; }
        [Required]
        [MaxLength(20, ErrorMessage = "Region Name allows up to 20 characters only") ]
        public string Name { get; set; }

        public string? RegionImageUrl { get; set; }
    }
}
