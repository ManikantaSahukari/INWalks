using System.Linq;
using AutoMapper;
using INWalksAPI.CustomActionFilter;
using INWalksAPI.Data;
using INWalksAPI.Models.Domain;
using INWalksAPI.Models.DTO;
using INWalksAPI.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace INWalksAPI.Controllers
{
    /*
     * Note:
     * Automapper syntax
     * var regionDto= mapper.Map<List<Destination>>(source);
     */
    //localhost:xxxx/api/regions
    [Route("api/[controller]")]
    [ApiController]
    
    public class RegionsController : Controller
    {
        private readonly INWalksDbContext dbContext;
        private readonly IRegionRepository regionRepository;// added as part of Implementing Repository pattern
        private readonly IMapper mapper;

        //public RegionsController(INWalksDbContext dbContext) Made a change as part of Repository pattern
        public RegionsController(INWalksDbContext dbContext, 
            IRegionRepository regionRepository,
            IMapper mapper
            )
        { 
            this.dbContext=dbContext;
            this.regionRepository = regionRepository; // added as part of Implementing Repository pattern
            this.mapper = mapper;
        }

        #region// GET ALL REGION DETAILS
        [HttpGet]
        [Authorize(Roles = "Reader")]
        public async Task<IActionResult> GetAll() //For async programming
        //public IActionResult GetAll()
        {
            #region//Get Data From Database - Domain Models
            //var regionDomain =  dbContext.Regions.ToList();
            //var regionDomain = await dbContext.Regions.ToListAsync();//For async programming
            var regionDomain = await  regionRepository.GetAllAsync();//// made a change as part of Implementing Repository pattern
            #endregion
            #region//Mapping without Automapper Domain Model to DTOs
            /*var regionDto = new List<RegionDto>();
            foreach (var region in regionDomain)
            {
                regionDto.Add(new RegionDto()
                {
                    Id = region.Id,
                    Code = region.Code,
                    Name = region.Name,
                    RegionImageUrl = region.RegionImageUrl
                });

            }*/
            #endregion

            #region //Mapping with Automapper Domain Model to DTO
            var regionDto= mapper.Map<List<RegionDto>>(regionDomain);
            #endregion
            //return DTO
            return Ok(regionDto);
        }   


        //GET THE REGION BY ID
        [HttpGet]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Reader")]
        public async Task<IActionResult> GetById([FromRoute]Guid id)
        {
            //Get Data From Database - Domain Models
            // var regionDomain = dbContext.Regions.Find(id);
            //var regionDomain = await dbContext.Regions.FirstOrDefaultAsync(x=> x.Id==id);
            var regionDomain = await regionRepository.GetByIdAsync(id);
            if (regionDomain == null)
            {                
                return NotFound();
            }
            #region//Map without Automapper Domain Model to DTOs
            /*var regionDto = new RegionDto
            {
                Id = regionDomain.Id,
                Code = regionDomain.Code,
                Name = regionDomain.Name,
                RegionImageUrl = regionDomain.RegionImageUrl
            };*/
            #endregion
            
            //return DTO to client
            return Ok(mapper.Map<RegionDto>(regionDomain));
        }


        //CREATE THE REGION
        [HttpPost]
        [ValidationModel]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> Create([FromBody] AddRegionRequestDto addRegionRequestDto)
        {
            /*if (ModelState.IsValid)
            {
            }
            else
            {
                return BadRequest(ModelState);// it will througth the 400 Error
            }*/
            #region//Mapping without AutoMapper the DTO to Domain Mode
            /*var regionDominModel = new Region
            {
                Code = addRegionRequestDto.Code,
                Name=addRegionRequestDto.Name,
                RegionImageUrl=addRegionRequestDto.RegionImageUrl
            };*/
            #endregion

            var regionDominModel = mapper.Map<Region>(addRegionRequestDto);
            //Use Domain Model To create the Region
            //await dbContext.Regions.AddAsync(regionDominModel);
            //await dbContext.SaveChangesAsync();
            regionDominModel = await regionRepository.CreateAsync(regionDominModel);

            #region//Map without AutoMapper Domain Model Back to DTO
            /*var regionDto = new RegionDto
            {
                Id = regionDominModel.Id,
                Code = regionDominModel.Code,
                Name = regionDominModel.Name,
                RegionImageUrl = regionDominModel.RegionImageUrl
            };*/
            #endregion
            //Mapped with AutoMapper 
            var regionDto = mapper.Map<RegionDto>(regionDominModel);
            //This method is used to return a 201 Created response along with a Location header pointing to the newly created resource.
            return CreatedAtAction(nameof(GetById), new { id = regionDominModel.Id }, regionDto);

        }

        //UPDATE THE REGION
        [HttpPut]
        [Route("{id:Guid}")]
        [ValidationModel]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRegionRequestDto updateRegionRequestDto)
        {
            #region//MAP DTO to Domain model to pass object to repository method
            /*var regionDomainModel = new Region
            {
                Code = updateRegionRequestDto.Code,
                Name = updateRegionRequestDto.Name,
                RegionImageUrl = updateRegionRequestDto.RegionImageUrl
            };*/
            #endregion
            var regionDomainModel = mapper.Map<Region>(updateRegionRequestDto);
            //Check if Region is Exists or not
            //var regionDomainModel = await dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
            regionDomainModel = await regionRepository.UpdateAsync(id, regionDomainModel);
            if (regionDomainModel == null)
            {
                return NotFound();
            }
            #region//Without Repository
            //regionDomainModel.Name = updateRegionRequestDto.Name;
            //regionDomainModel.Code = updateRegionRequestDto.Code;
            //regionDomainModel.RegionImageUrl = updateRegionRequestDto.RegionImageUrl;
            //await dbContext.SaveChangesAsync();
            #endregion

            #region//Map  Domain Model to DTO
            /*var regionDto = new RegionDto
            {

                Id = regionDomainModel.Id,
                Code = regionDomainModel.Code,
                Name = regionDomainModel.Name,
                RegionImageUrl = regionDomainModel.RegionImageUrl
            };*/
            #endregion
            //Convert Domain
            var regionDto = mapper.Map<RegionDto>(regionDomainModel);
            return Ok(regionDto);
        }
        
        //DELETE THE REGION
        [HttpDelete]
        //[Route("id:Guid")] I made a mistake here
        [Route("{id:Guid}")]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            //var regionDomainModel = await dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
            var regionDomainModel = await regionRepository.DeleteAsync(id);
            if (regionDomainModel == null)
            {
                return NotFound();
            }
            //dbContext.Regions.Remove(regionDomainModel);
            //await dbContext.SaveChangesAsync();
            #region//Map the region Domine to Dto
            /*var regionDto = new RegionDto
            {

                Id = regionDomainModel.Id,
                Code = regionDomainModel.Code,
                Name = regionDomainModel.Name,
                RegionImageUrl = regionDomainModel.RegionImageUrl
            };*/
            #endregion

            return Ok(mapper.Map<RegionDto>(regionDomainModel));

        }
    }
}
