using Microsoft.AspNetCore.JsonPatch;
using MissingPersonAPI.Services;

namespace MissingPersonAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class LostPersonController : ControllerBase
    {
      
        private readonly ILostPersonService _lostPersonService;
        public LostPersonController(ILostPersonService lostPersonService)
        {
            _lostPersonService = lostPersonService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllLostPersons()
        {
            var LostPerson = await _lostPersonService.GetAllLostPersons();
            return Ok(LostPerson);
        }
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
         var  LostPerson = await _lostPersonService.GetById(id);
            return Ok(LostPerson);
        }

        [HttpPost]
        public async Task<IActionResult> PostLostperson([FromForm] LostPersonWithUserDTO lDTO)

        {
            var foundPerson = await _lostPersonService.PostLostPerson(lDTO);
            return Ok(foundPerson);
        }


        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromForm] LostPersonWithUserDTO lNewDTO)
        {
           var NewPerson = await _lostPersonService.Update(id, lNewDTO);
            return Ok(NewPerson);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteAsync(int id)
        { 
            var lostprs = await _lostPersonService.Delete(id);
            return Ok(lostprs);
        }
    }
}
