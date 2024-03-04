using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using MissingPersonAPI.Resources;

namespace MissingPersonAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LanguageController : ControllerBase
    {
        private readonly IStringLocalizer<SharedResource> _Localizer;
        public LanguageController(IStringLocalizer<SharedResource> Localizer)
        {
            
            _Localizer = Localizer;
        }
        [HttpGet]
    public IActionResult get()
        {
            var massege = _Localizer.GetString("Hello").Value ?? "";
            var change = _Localizer["Hello"];
            return Ok(change);
        }

    }
}
