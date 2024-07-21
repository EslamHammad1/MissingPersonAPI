using MissingPersonAPI.Services;

namespace MissingPersonAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FoundPersonController : ControllerBase
    {
              private readonly IFoundPersonService _foundPersonService;
        public FoundPersonController( IFoundPersonService foundPersonService)
        {
         _foundPersonService = foundPersonService;
        }

        [HttpGet]
        public async  Task<IActionResult> GetAllFoundPerson()
        {
            var FoundList = await _foundPersonService.GetAllFoundPerson();
            return Ok(FoundList);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id )
        {
            var foundPrs = await _foundPersonService.GetById(id);
                return Ok(foundPrs);
        }

        [HttpPost]
        public async Task<IActionResult> PostFoundperson([FromForm] FoundPersonWithUserDTO fDTO)

        {
            var foundPerson= await _foundPersonService.PostFoundPerson(fDTO);
            return Ok(foundPerson);
        }
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromForm] FoundPersonWithUserDTO fNewDTO)
        {
            var FoundPerson = await _foundPersonService.Update(id, fNewDTO);
            return Ok(FoundPerson);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
         var foundPrs = await _foundPersonService.Delete(id);
            return Ok(foundPrs);
        }
    }
}
