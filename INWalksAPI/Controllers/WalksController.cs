using AutoMapper;
using INWalksAPI.CustomActionFilter;
using INWalksAPI.Models.Domain;
using INWalksAPI.Models.DTO;
using INWalksAPI.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace INWalksAPI.Controllers
{
    //api/walks
    [Route("api/[controller]")]
    [ApiController]
    public class WalksController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IwalkRepository walkRepository;
        public WalksController(IMapper mapper, IwalkRepository walkRepository)
        {
            this.mapper = mapper;
            this.walkRepository = walkRepository;
        }
        //Create Walks
        //Post://api/walks
        [HttpPost]
        [ValidationModel]
        public async Task<IActionResult> Create([FromBody] AddWalkRequestdto addWalkRequestdto)
        {
            //Map DTO to Domine MOdel
            var WalkDomainModel = mapper.Map<Walk>(addWalkRequestdto);
            await walkRepository.CreateAsync(WalkDomainModel);
            //Map
            return Ok(mapper.Map<WalkDto>(WalkDomainModel));

        }

        //TO GET ALL WALKS DETAILS
        //GET: /API/WALKS?Filteron=Name&filterQuery=Track
        [HttpGet]
        public async Task<IActionResult> GetAllWalks([FromQuery] string? filterOn
                                                    ,[FromQuery] string? filterQuery
                                                    ,[FromQuery] string? sortBy
                                                    ,[FromQuery] bool? isAscending
                                                    ,[FromQuery] int pageNumber=1
                                                    ,[FromQuery] int pageSize=1000
                                                    )
        {
            var walks = await walkRepository.GetAllWalksAsync(filterOn, filterQuery, sortBy, isAscending ?? true,pageNumber, pageSize);
            var walkDtos = mapper.Map<List<WalkDto>>(walks);
            return Ok(walkDtos);
        }


        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetWalksByID([FromRoute] Guid id)
        {
            var WalkDomineModel = await walkRepository.GetWalksByIdAsync(id);

            if (WalkDomineModel == null)
            {
                return NotFound();
            }
            return Ok(mapper.Map<WalkDto>(WalkDomineModel));

        }

        [HttpPut]
        [Route("{id:Guid}")]
        [ValidationModel]
        public async Task<IActionResult> UpdateWalkRequst([FromRoute] Guid id, [FromBody] UpdateWalkRequestDto updateWalkRequestDto)
        {
            //Map DTO to Domain
            var walkDataDomine = mapper.Map<Walk>(updateWalkRequestDto);

            walkDataDomine = await walkRepository.UpdateWalkRequstAsync(id, updateWalkRequestDto);
            if (walkDataDomine == null)
            {
                return NotFound();
            }
            return Ok(mapper.Map<WalkDto>(walkDataDomine));
        }
        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteWalkById([FromRoute] Guid id)
        {
            var deletedWalkDomineModel = await walkRepository.GetWalksByIdAsync(id);
            if (deletedWalkDomineModel == null)
            {
                return NotFound();
            }
            //Map Domine model to DTo
            return Ok(mapper.Map<WalkDto>(deletedWalkDomineModel));
        }
    }
}
