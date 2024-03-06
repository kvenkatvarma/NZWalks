using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.Api.Models.Domain;
using NZWalks.Api.Models.DTO;
using NZWalks.Api.Repositories;

namespace NZWalks.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalksController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IWalkRepository walkRepository;

        public WalksController(IMapper mapper,IWalkRepository walkRepository)
        {
            this.mapper = mapper;
            this.walkRepository = walkRepository;
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddWalkRequestDto addWalkRequestDto) 
        {
           var walkDomaimModel= mapper.Map<Walk>(addWalkRequestDto);
           await walkRepository.CreateAsync(walkDomaimModel);
            return Ok(mapper.Map<WalkDto>(walkDomaimModel));
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? filtercolumnname =null,string? filtervalue = null,string? sortBy = null,bool IsAscending=true, int pageNumber = 1, int pageSize = 1000)
        {
            var walks=await walkRepository.GetAllAsync(filtercolumnname, filtervalue, sortBy, IsAscending, pageNumber,pageSize);
            throw new Exception("This is a ne exception");
            return Ok(mapper.Map<List<WalkDto>>(walks));
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetByID([FromRoute] Guid id)
        {
           var walk= await walkRepository.GetById(id);
            if(walk == null)
            {
                return NotFound();
            }
            return Ok(mapper.Map<WalkDto>(walk));
        }
        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, UpdateWalkRequestDto updateWalkRequestDto)
        {
            var walk = mapper.Map<Walk>(updateWalkRequestDto);
            var walkmodel=await walkRepository.Update(id, walk);
            return Ok(mapper.Map<WalkDto>(walkmodel));
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
           var walk= await walkRepository.Delete(id);
            return Ok(mapper.Map<WalkDto>(walk));

        }
    }
}
