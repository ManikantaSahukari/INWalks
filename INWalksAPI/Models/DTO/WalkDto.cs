namespace INWalksAPI.Models.DTO
{
    public class WalkDto
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public double LengthInKm { get; set; }

        public string? WalkImageUrl { get; set; }

        //public Guid DifficultID { get; set; }

        //public Guid RegionId { get; set; }
        public RegionDto region { get; set; }

        public DifficultyDto Difficulty { get; set; }



    }
}
