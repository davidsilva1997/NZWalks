using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WalkDifficultiesController : Controller
    {
        private readonly IWalkDifficultyRepository walkDifficultyRepository;
        private readonly IMapper mapper;

        public WalkDifficultiesController(IWalkDifficultyRepository walkDifficultyRepository, IMapper mapper)
        {
            this.walkDifficultyRepository = walkDifficultyRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllWalkDifficultiesAsync()
        {
            var walkDifficulties = await walkDifficultyRepository.GetAllAsync();

            var walkDifficultiesDTO = mapper.Map<IEnumerable<Models.DTO.WalkDifficulty>>(walkDifficulties);

            return Ok(walkDifficultiesDTO);
        }

        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetWalkDifficulty")]
        public async Task<IActionResult> GetWalkDifficulty([FromRoute] Guid id)
        {
            var walkDifficulty = await walkDifficultyRepository.GetAsync(id);

            if (walkDifficulty is null)
            {
                return NotFound();
            }

            var walkDifficultyDTO = mapper.Map<Models.DTO.WalkDifficulty>(walkDifficulty);

            return Ok(walkDifficultyDTO);
        }

        [HttpPost]
        public async Task<IActionResult> AddWalkDifficultyAsync([FromBody] Models.DTO.WalkDifficultyRequest walkDifficultyRequest)
        {
            if (!ValidateAddWalkDifficultyAsync(walkDifficultyRequest))
            {
                return BadRequest(ModelState);
            }

            var walkDifficulty = new Models.Domain.WalkDifficulty()
            {
                Code = walkDifficultyRequest.Code
            };

            walkDifficulty = await walkDifficultyRepository.AddAsync(walkDifficulty);

            var walkDifficultyDTO = mapper.Map<Models.DTO.WalkDifficulty>(walkDifficulty);

            return CreatedAtAction(nameof(GetWalkDifficulty), new { id = walkDifficulty.Id }, walkDifficultyDTO);
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> RemoveWalkDifficultyAsync([FromRoute] Guid id)
        {
            var walkDifficulty = await walkDifficultyRepository.RemoveAync(id);

            if (walkDifficulty is null)
            {
                return NotFound();
            }

            var walkDifficultyDTO = mapper.Map<Models.DTO.WalkDifficulty>(walkDifficulty);

            return Ok(walkDifficultyDTO);
        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateWalkDifficultyAsync([FromRoute] Guid id, [FromBody] Models.DTO.WalkDifficultyRequest walkDifficultyRequest)
        {
            if (!ValidateUpdateWalkDifficultyAsync(walkDifficultyRequest))
            {
                return BadRequest(ModelState);
            }

            var updatedWalkDifficulty = new Models.Domain.WalkDifficulty()
            {
                Code = walkDifficultyRequest.Code
            };

            var walkDifficulty = await walkDifficultyRepository.UpdateAsync(id, updatedWalkDifficulty); 

            if (walkDifficulty is null)
            {
                return NotFound();
            }

            var walkDifficultyDTO = mapper.Map<Models.DTO.WalkDifficulty>(walkDifficulty);

            return Ok(walkDifficultyDTO);
        }

        #region Private methods

        private bool ValidateAddWalkDifficultyAsync(Models.DTO.WalkDifficultyRequest walkDifficultyRequest)
        {
            if (walkDifficultyRequest is null)
            {
                ModelState.AddModelError(nameof(walkDifficultyRequest), string.Format("{0} must contain Walk Difficulty data.", nameof(walkDifficultyRequest)));
                return false;
            }

            if (string.IsNullOrWhiteSpace(walkDifficultyRequest.Code))
            {
                ModelState.AddModelError(nameof(walkDifficultyRequest.Code), string.Format("{0} cannot be null or empty or white space.", nameof(walkDifficultyRequest.Code)));
            }

            if (ModelState.ErrorCount > 0)
            {
                return false;
            }

            return true;
        }

        private bool ValidateUpdateWalkDifficultyAsync(Models.DTO.WalkDifficultyRequest walkDifficultyRequest)
        {
            if (walkDifficultyRequest is null)
            {
                ModelState.AddModelError(nameof(walkDifficultyRequest), string.Format("{0} must contain Walk Difficulty data.", nameof(walkDifficultyRequest)));
                return false;
            }

            if (string.IsNullOrWhiteSpace(walkDifficultyRequest.Code))
            {
                ModelState.AddModelError(nameof(walkDifficultyRequest.Code), string.Format("{0} cannot be null or empty or white space.", nameof(walkDifficultyRequest.Code)));
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
