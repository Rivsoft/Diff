using AutoMapper;
using Diff.API.DTOs;
using Diff.API.Helpers;
using Diff.Core.Integration.Messages;
using Diff.Data.Repositories;
using EasyNetQ;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Diff.API.Controllers
{
    [Route("/v1/[controller]")]
    [ApiController]
    public class DiffController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IBus _serviceBus;
        private readonly IDiffAnalysisRepository _diffRepo;

        public DiffController(IBus serviceBus, IMapper mapper, IDiffAnalysisRepository diffRepository)
        {
            _serviceBus = serviceBus;
            _mapper = mapper;
            _diffRepo = diffRepository;
        }

        [HttpPost("{id}/left")]
        public async Task<IActionResult> AddLeft(Guid id, [FromBody] string input)
        {
            return await AddInputArgument(id, input, true);
        }

        [HttpPost("{id}/right")]
        public async Task<IActionResult> AddRight(Guid id, [FromBody] string input)
        {
            return await AddInputArgument(id, input, false);
        }

        private async Task<IActionResult> AddInputArgument(Guid id, string input, bool isLeftArgument)
        {
            byte[] inputArray = Base64Helper.ConvertBase64String(input);

            if (id == Guid.Empty || inputArray == null)
                return BadRequest();

            var message = new AddDiffInputMessage { Id = id, Input = inputArray, IsLeft = isLeftArgument };

            await _serviceBus.PublishAsync(message);

            var uri = Url.RouteUrl("GetDiffResult", new { id });

            return Accepted(uri);
        }

        /// <summary>
        /// Returns a diff analysis.
        /// </summary>
        /// <param name="id">The <see cref="Guid"/> of the analysis to be retrieved</param>
        /// <returns>
        ///     A <see cref="GetDiffAnalysisForResultDTO"/> model (in JSON format) representing the analized data. 
        ///     If the <see cref="GetDiffAnalysisForResultDTO.Analyzed"/> atribute is false it means the analysis has not been completed yet.
        /// </returns>
        [HttpGet("{id}", Name = "GetDiffResult")]
        public async Task<IActionResult> GetDiffResult(Guid id)
        {
            var result = await _diffRepo.GetAnalysis(id);

            if (result != null) // check if analysis exists
            {
                var response = _mapper.Map<GetDiffAnalysisForResultDTO>(result);

                return Ok(response);
            }
            else // analysis doesnt exist
            {
                return NotFound();
            }
        }
    }
}