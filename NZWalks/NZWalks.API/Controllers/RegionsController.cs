using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.Domain;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RegionsController : Controller
    {
        private readonly IRegionRepository regionRepository;
        private readonly IMapper mapper;

        public RegionsController(IRegionRepository regionRepository, IMapper mapper)
        {
            this.regionRepository = regionRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRegionsAsync()
        {
            var regions = await regionRepository.GetAllAsync();

            // return DTO regions
            var regionsDTO = mapper.Map<List<Models.DTO.Region>>(regions);

            return Ok(regionsDTO);
        }

        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetRegionAsync")]
        public async Task<IActionResult> GetRegionAsync(Guid id)
        {
            var region = await regionRepository.GetAsync(id);

            if (region is null)
            {
                return NotFound();
            }

            var regionDTO = mapper.Map<Models.DTO.Region>(region);

            return Ok(regionDTO);
        }

        [HttpPost]
        public async Task<IActionResult> AddRegionAsync(Models.DTO.RegionRequest regionRequest)
        {
            // Validate the request
            if (!ValidateAddRegionAsync(regionRequest))
            {
                return BadRequest(ModelState);
            }

            // Convert to Domain model
            var region = new Models.Domain.Region()
            {
                Code = regionRequest.Code,
                Area = regionRequest.Area,
                Lat = regionRequest.Lat,
                Long = regionRequest.Long,
                Name = regionRequest.Name,
                Population = regionRequest.Population
            };

            // Pass details to Repository
            region = await regionRepository.AddAsync(region);

            // Convert back to DTO
            var regionDTO = mapper.Map<Models.DTO.Region>(region);

            return CreatedAtAction(nameof(GetRegionAsync), new { id = regionDTO.Id }, regionDTO);
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> RemoveRegionAsync(Guid id)
        {
            var region = await regionRepository.RemoveAsync(id);

            if (region is null)
            {
                return NotFound();
            }

            var regionDTO = mapper.Map<Models.DTO.Region>(region);

            return Ok(regionDTO);
        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateRegionAsync([FromRoute] Guid id, [FromBody] Models.DTO.RegionRequest regionRequest)
        {
            // validate region
            if (!ValidateUpdateRegionAsync(regionRequest))
            {
                return BadRequest(ModelState);
            }

            // Convert to Domain model
            var updatedRegion = new Models.Domain.Region()
            {
                Code = regionRequest.Code,
                Area = regionRequest.Area,
                Lat = regionRequest.Lat,
                Long = regionRequest.Long,
                Name = regionRequest.Name,
                Population = regionRequest.Population
            };

            var region = await regionRepository.UpdateAsync(id, updatedRegion);

            if (region is null)
            {
                return NotFound();
            }

            var regionDTO = mapper.Map<Models.DTO.Region>(region);

            return Ok(regionDTO);
        }

        #region Private methods

        private bool ValidateAddRegionAsync(Models.DTO.RegionRequest regionRequest)
        {
            if (regionRequest is null)
            {
                ModelState.AddModelError(nameof(regionRequest), string.Format("{0} must contain Region data.", nameof(regionRequest)));
                return false;
            }

            if (string.IsNullOrWhiteSpace(regionRequest.Code))
            {
                ModelState.AddModelError(nameof(regionRequest.Code), string.Format("{0} cannot be null or empty or white space.", nameof(regionRequest.Code)));
            }

            if (string.IsNullOrWhiteSpace(regionRequest.Name))
            {
                ModelState.AddModelError(nameof(regionRequest.Name), string.Format("{0} cannot be null or empty or white space.", nameof(regionRequest.Name)));
            }

            if (regionRequest.Area <= 0)
            {
                ModelState.AddModelError(nameof(regionRequest.Area), string.Format("{0} cannot be less or equal to zero.", nameof(regionRequest.Area)));
            }

            if (regionRequest.Population < 0)
            {
                ModelState.AddModelError(nameof(regionRequest.Population), string.Format("{0} cannot be less than zero.", nameof(regionRequest.Population)));
            }

            if (ModelState.ErrorCount > 0)
            {
                return false;
            }

            return true;
        }

        private bool ValidateUpdateRegionAsync(Models.DTO.RegionRequest regionRequest)
        {
            if (regionRequest is null)
            {
                ModelState.AddModelError(nameof(regionRequest), string.Format("{0} must contain Region data.", nameof(regionRequest)));
                return false;
            }

            if (string.IsNullOrWhiteSpace(regionRequest.Code))
            {
                ModelState.AddModelError(nameof(regionRequest.Code), string.Format("{0} cannot be null or empty or white space.", nameof(regionRequest.Code)));
            }

            if (string.IsNullOrWhiteSpace(regionRequest.Name))
            {
                ModelState.AddModelError(nameof(regionRequest.Name), string.Format("{0} cannot be null or empty or white space.", nameof(regionRequest.Name)));
            }

            if (regionRequest.Area <= 0)
            {
                ModelState.AddModelError(nameof(regionRequest.Area), string.Format("{0} cannot be less or equal to zero.", nameof(regionRequest.Area)));
            }

            if (regionRequest.Population < 0)
            {
                ModelState.AddModelError(nameof(regionRequest.Population), string.Format("{0} cannot be less than zero.", nameof(regionRequest.Population)));
            }

            if (ModelState.ErrorCount > 0)
            {
                return false;
            }

            return true;
        }

        #endregion
    }
}
