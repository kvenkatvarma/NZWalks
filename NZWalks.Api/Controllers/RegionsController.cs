using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.Api.CustomValidations;
using NZWalks.Api.Data;
using NZWalks.Api.Models.Domain;
using NZWalks.Api.Models.DTO;
using NZWalks.Api.Repositories;
using System.Text.Json;

namespace NZWalks.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
   
    public class RegionsController : ControllerBase
    {
        private readonly NZWalksDbContext _dbcontext;
        private readonly IRegionRepository regionrepo;
        private readonly IMapper mapper;
        private readonly ILogger<RegionsController> logger;

        public RegionsController(NZWalksDbContext dbcontext,IRegionRepository regionRepository,IMapper mapper,ILogger<RegionsController> logger)
        {
            this._dbcontext = dbcontext;
            this.regionrepo = regionRepository;
            this.mapper = mapper;
            this.logger = logger;
        }
        [HttpGet]
        //[Authorize(Roles ="Reader")]
        public  async Task<IActionResult> GetAll()
        {
            
                //Here we are getting Region Domain Model from Database
                var regionsDomain = await regionrepo.GetAllAsync();
                //MApping the Region Model to Region DTO which will be exposed to client

                //var regionDto = new List<RegionsDto>();

                //foreach(var regionDomain in regionsDomain)
                //{
                //    regionDto.Add(new RegionsDto
                //    {
                //        Id = regionDomain.Id,
                //        Code=regionDomain.Code,
                //        Name=regionDomain.Name,
                //        RegionImageUrl=regionDomain.RegionImageUrl
                //    });
                //}
              
                var regionDto = mapper.Map<List<RegionsDto>>(regionsDomain);
                return Ok(regionDto);
           
        }

        [HttpGet]
        [Route("{id:Guid}")]
        //[Authorize(Roles = "Reader")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            //Here we can use find only on primary key properties but not on other properties
            // var region = _dbcontext.Regions.Find(id);

            var region = await regionrepo.GetByIDAsync(id);
            
            if(region == null)
            {
                return NotFound();
            }
            //Mapping Domain Model to DTO
            //var regionDto = new RegionsDto
            //{
            //    Id = region.Id,
            //    Code = region.Code,
            //    Name = region.Name,
            //    RegionImageUrl = region.RegionImageUrl
            //};
            var regionDto = mapper.Map<RegionsDto>(region);
            return Ok(regionDto);
        }

        [HttpPost]
        //[CustomValidations]
        //[Authorize(Roles = "Writer")]
        public async Task<IActionResult> Create([FromBody] AddRegionRequestDto regionDto)
        {
            //Mapping the Region Dto to Region Domain
            //var region = new Region
            //{
            //    Code=regionDto.Code,
            //    Name=regionDto.Name,
            //    RegionImageUrl=regionDto.RegionImageUrl
            //};
            //Instead of this we can use custom validations
            //if (ModelState.IsValid)
            //{
            var region = mapper.Map<Region>(regionDto);
            //Saving the Region Model to Database
            region = await regionrepo.CreateAsync(region);
            //Instead of exposing the Region Model to client we will map the region to RegionDto 
            //var regionDto1 = new RegionsDto
            //{
            //    Code = region.Code,
            //    Name= region.Name,
            //    RegionImageUrl=region.RegionImageUrl,
            //    Id=region.Id
            //};
            var regionDto1 = mapper.Map<RegionsDto>(region);

            return CreatedAtAction(nameof(GetById), new { id = regionDto1.Id }, regionDto1);
            //}
            //else
            //{
            //    return BadRequest(ModelState);
            //}

        }

        [HttpPut]
        [Route("{id:Guid}")]
        //[CustomValidations]
        //[Authorize(Roles = "Writer")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRegionRequestDto updateRegionDto)
        {
            //var region1 = new Region
            //{
            //    Code = updateRegionDto.Code,
            //    Name = updateRegionDto.Name,
            //    RegionImageUrl = updateRegionDto.RegionImageUrl
            //};
            //if (ModelState.IsValid)
            //{

                var region1 = mapper.Map<Region>(updateRegionDto);

                var region = await regionrepo.UpdateAsync(id, region1);
                if (region == null)
                {
                    return NotFound();
                }

                //Mapping the Region Domain to Dto

                //var regionDto= new RegionsDto
                //{
                //    Name=region.Name,
                //    Code=region.Code,
                //    RegionImageUrl=region.RegionImageUrl
                //};
                var regionDto = mapper.Map<RegionsDto>(region);
                return Ok(regionDto);
            //}
            //else
            //{
            //    return BadRequest(ModelState);
            //}

        }

        [HttpDelete]
        [Route("{id:Guid}")]
        //[Authorize(Roles = "Writer")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var region = await regionrepo.DeleteAsync(id);
            if(region == null)
            {
                return NotFound();
            }
            ;
            //If we want we can send the deleted item by mapping the model to DTO or else we can simply send emoty response
            //var regionDto = new RegionsDto
            //{
            //    Name = region.Name,
            //    Code = region.Code,
            //    RegionImageUrl = region.RegionImageUrl
            //};
            var regionDto = mapper.Map<RegionsDto>(region);
            return Ok(regionDto);
        }
       
    }
}
