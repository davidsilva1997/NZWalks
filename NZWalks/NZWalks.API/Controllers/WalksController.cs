using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;
using System.Data;
using System.Formats.Asn1;

namespace NZWalks.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WalksController : Controller
    {
        private readonly IWalkRepository walkRepository;
        private readonly IMapper mapper;
        private readonly IRegionRepository regionRepository;
        private readonly IWalkDifficultyRepository walkDifficultyRepository;

        public WalksController(IWalkRepository walkRepository, IMapper mapper, IRegionRepository regionRepository, IWalkDifficultyRepository walkDifficultyRepository)
        {
            this.walkRepository = walkRepository;
            this.mapper = mapper;
            this.regionRepository = regionRepository;
            this.walkDifficultyRepository = walkDifficultyRepository;
        }

        [HttpGet]
        [Authorize(Roles = "reader")]
        public async Task<IActionResult> GetAllWalksAsync()
        {
            var walks = await walkRepository.GetAllAsync();

            var walksDTO = mapper.Map<List<Models.DTO.Walk>>(walks);

            return Ok(walksDTO);
        }

        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetWalkAsync")]
        [Authorize(Roles = "reader")]
        public async Task<IActionResult> GetWalkAsync(Guid id)
        {
            var walk = await walkRepository.GetAsync(id);

            if (walk is null)
            {
                return NotFound();
            }

            var walkDTO = mapper.Map<Models.DTO.Walk>(walk);

            return Ok(walkDTO);
        }

        [HttpPost]
        [Authorize(Roles = "writer")]
        public async Task<IActionResult> AddWalkAsync([FromBody] Models.DTO.WalkRequest walkRequest)
        {
            if (!(await ValidateAddWalkAsync(walkRequest)))
            {
                return BadRequest(ModelState);
            }

            var walk = new Models.Domain.Walk()
            {
                Name = walkRequest.Name,
                Length = walkRequest.Length,
                RegionId = walkRequest.RegionId,
                WalkDifficultyId = walkRequest.WalkDifficultyId,
            };

            walk = await walkRepository.AddAsync(walk);

            var walkDTO = mapper.Map<Models.DTO.Walk>(walk);

            return CreatedAtAction(nameof(GetWalkAsync), new { id = walkDTO.Id }, walkDTO);
        }

        [HttpDelete]
        [Route("{id:guid}")]
        [Authorize(Roles = "writer")]
        public async Task<IActionResult> RemoveWalkAsync(Guid id)
        {
            var walk = await walkRepository.RemoveAsync(id);

            if (walk is null)
            {
                return NotFound();
            }

            var walkDTO = mapper.Map<Models.DTO.Walk>(walk);

            return Ok(walkDTO);
        }

        [HttpPut]
        [Route("{id:guid}")]
        [Authorize(Roles = "writer")]
        public async Task<IActionResult> UpdateWalkAsync([FromRoute] Guid id,[FromBody] Models.DTO.WalkRequest request)
        {
            if (!(await ValidateUpdateWalkAsync(request)))
            {
                return BadRequest(ModelState);
            }

            var updatedWalk = new Models.Domain.Walk()
            {
                Name = request.Name,
                Length = request.Length,
                RegionId = request.RegionId,
                WalkDifficultyId = request.WalkDifficultyId
            };

            var walk = await walkRepository.UpdateAsync(id, updatedWalk);

            if (walk is null)
            {
                return NotFound();
            }

            var walkDTO = mapper.Map<Models.DTO.Walk>(walk);

            return Ok(walkDTO);
        }

        #region Private methods

        private async Task<bool> ValidateAddWalkAsync(Models.DTO.WalkRequest walkRequest)
        {
            var region = await regionRepository.GetAsync(walkRequest.RegionId);
            if (region is null)
            {
                ModelState.AddModelError(nameof(walkRequest.RegionId), string.Format("{0} is not a valid region.", nameof(walkRequest.RegionId)));
            }

            var walkDifficulty = await walkDifficultyRepository.GetAsync(walkRequest.WalkDifficultyId);
            if (walkDifficulty is null)
            {
                ModelState.AddModelError(nameof(walkRequest.WalkDifficultyId), string.Format("{0} is not a valid walk difficulty.", nameof(walkRequest.WalkDifficultyId)));
            }

            if (ModelState.ErrorCount > 0)
            {
                return false;
            }

            return true;
        }

        private async Task<bool> ValidateUpdateWalkAsync(Models.DTO.WalkRequest walkRequest)
        {          
            var region = await regionRepository.GetAsync(walkRequest.RegionId);
            if (region is null)
            {
                ModelState.AddModelError(nameof(walkRequest.RegionId), string.Format("{0} is not a valid region.", nameof(walkRequest.RegionId)));
            }

            var walkDifficulty = await walkDifficultyRepository.GetAsync(walkRequest.WalkDifficultyId);
            if (walkDifficulty is null)
            {
                ModelState.AddModelError(nameof(walkRequest.WalkDifficultyId), string.Format("{0} is not a valid walk difficulty.", nameof(walkRequest.WalkDifficultyId)));
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
