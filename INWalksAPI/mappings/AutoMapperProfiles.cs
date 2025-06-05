using AutoMapper;
using INWalksAPI.Models.Domain;
using INWalksAPI.Models.DTO;

namespace INWalksAPI.mappings
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Region,RegionDto>().ReverseMap();
            CreateMap<AddRegionRequestDto, Region>().ReverseMap();
            CreateMap<Region, UpdateRegionRequestDto>().ReverseMap();
            CreateMap<AddWalkRequestdto,Walk>().ReverseMap();
            CreateMap<Walk,WalkDto>().ReverseMap();
            CreateMap<Difficulty,DifficultyDto>().ReverseMap();
            CreateMap<UpdateWalkRequestDto, Walk>().ReverseMap();
            //CreateMap<UpdateRegionRequestDto, WalkDto>().ReverseMap();
        }
    }
}
