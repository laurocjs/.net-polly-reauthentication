using Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MyController : ControllerBase
    {
        private readonly ISomeService _someService;

        public MyController(ISomeService someService)
        {
            _someService = someService;
        }

        /// <summary>
        /// Just any get, list your available resources
        /// </summary>
        /// <returns>A list with your resources</returns>
        [HttpGet]
        [Route("")]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> List()
        {
            var result = await _someService.GetSomething();

            if (result != null && result.Any())
                return Ok(result);

            return NotFound("Couldn't retrieve the requested data");
        }
    }
}
