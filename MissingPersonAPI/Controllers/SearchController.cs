using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using MissingPersonAPI.Services;
using Size = SixLabors.ImageSharp.Size;
using Image = SixLabors.ImageSharp.Image;

namespace MissingPersonAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        private readonly ISearchService _searchService;
        private readonly MissingPersonEntity _context;
        private readonly HttpClient _httpClient;
        private double some_threshold;

        public SearchController(ISearchService searchService , MissingPersonEntity context , HttpClient HttpClient)
        {
            _searchService = searchService;
            _context = context;
            _httpClient = HttpClient;
        }
        [HttpGet("SearchByNameForLost")]
        public  IActionResult SearchForLostByName([FromQuery] SearchNameDTO searchDTO)
        {
           var LostPerson= _searchService.SearchForLostByName(searchDTO);
            return Ok(LostPerson);   
        }
        [HttpGet("SearchByCityForLost")]
        public IActionResult SearchForLostByCity([FromQuery] SearchCityDTO searchDTO)
        {
            var City = _searchService.SearchForLostByCity(searchDTO);
            return Ok(City);
        }
        [HttpGet("SearchByNameForFound")]
        public IActionResult SearchForFoundByName([FromQuery] SearchNameDTO searchDTO)
        {
            var FoundPrson = _searchService.SearchForFoundByName(searchDTO);
            return Ok(FoundPrson);
        }
        [HttpGet("SearchByCityForFound")]
        public IActionResult SearchForFoundByCity([FromQuery] SearchCityDTO searchDTO)
        {
            var City = _searchService.SearchForFoundByCity(searchDTO);
            return Ok(City);
        }

        [HttpPost("SearchByImage")]
        public async Task<IActionResult> SearchByImage([FromForm] SearchImage imageFile)
        {
            if (imageFile == null)
                return BadRequest("Image is required.");

            using var memoryStream = new MemoryStream();
            await imageFile.Iamge.CopyToAsync(memoryStream);
            var uploadedImage = memoryStream.ToArray();

            // Resize and process the uploaded image
            var processedUploaded = ProcessImage(uploadedImage);

            // Iterate through all images in the database
            var persons = await _context.foundPersons.OrderBy(i => i.Name).ToListAsync();
            var matchingPersons = new List<FoundPerson>(); // Assuming Person is your model class

            foreach (var person in persons)
            {
                var processedPersonImage = ProcessImage(person.Image);
                if (AreImagesSimilar(processedPersonImage, processedUploaded))
                {
                    matchingPersons.Add(person);
                }
            }

            if (matchingPersons.Count == 0)
            {
                return Ok("No matching person found.");
            }

            return Ok(matchingPersons);
        }

        private byte[] ProcessImage(byte[] imageBytes)
        {
            using var image = Image.Load<Rgba32>(imageBytes);
            image.Mutate(x => x.Resize(new ResizeOptions
            {
                Size = new Size(224, 224), // Adjust the size as needed
                Mode = ResizeMode.Crop
            }));

            using var ms = new MemoryStream();
            image.SaveAsPng(ms);
            return ms.ToArray();
        }

        private bool AreImagesSimilar(byte[] image1, byte[] image2)
        {
            if (image1.Length != image2.Length)
                return false;

            // Compare pixel by pixel
            for (int i = 0; i < image1.Length; i++)
            {
                if (image1[i] != image2[i])
                    return false;
            }
            return true;
        }
    }
}
