namespace Test_1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class FoundPersonController : ControllerBase
    {
        private new List<string> allwoedExtentions = new List<string> { ".jpg , .png" }; // new
        private long MaxallwoedImageSize = 10485760; // new
        private readonly MissingPersonEntity _context;
        public FoundPersonController(MissingPersonEntity context)
        {
            _context = context;
        }
        [HttpGet]
        public async  Task<IActionResult> GetAllFoundPerson()
        {
            List<FoundPerson> FoundList = await _context.foundPersons.OrderBy(i=>i.Name).ToListAsync();
            return Ok(FoundList);
        }
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetbyID(int id )
        {


            var foundPrs = await _context.foundPersons.FindAsync(id);
            FoundPersonWithUserDTO fDTO = new FoundPersonWithUserDTO();
            if (foundPrs == null)
                return NotFound();
            else
                return Ok(foundPrs);
        }
        [HttpPost]
        public async Task<IActionResult> PostFoundperson([FromForm] FoundPersonWithUserDTO fDTO)

        {
                if (fDTO.Image == null)
                    return BadRequest("Image is Required !");
                if (fDTO.Image.Length > MaxallwoedImageSize)
                    return BadRequest("Max allowed size for image is 10 MB! ");
                using var dataStreem = new MemoryStream();
                await fDTO.Image.CopyToAsync(dataStreem);
                var FoundPerson = new FoundPerson
                {
                    Name = fDTO.Name,
                    Age = (int)fDTO.Age,
                    Gender = fDTO.Gender,
                    Image = dataStreem.ToArray(), // new
                    Note = fDTO.Note,
                    FoundCity = fDTO.FoundCity,
                    Address_City = fDTO.Address_City,
                    Date = (DateTime)fDTO.Date,
                    PersonWhoFoundhim = fDTO.PersonWhoFoundhim,
                    PhonePersonWhoFoundhim = fDTO.PhonePersonWhoFoundhim,
                };
                await _context.AddAsync(FoundPerson);
                _context.SaveChanges();
                return Ok(FoundPerson);
        
        }
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromForm] FoundPersonWithUserDTO fNewDTO)
        {
            // Find the existing FoundPerson entity by its ID
            var oldPrs = await _context.foundPersons.FindAsync(id);

            // Check if the entity exists
            if (oldPrs == null)
                return NotFound($"Not Found {id}");

            // Update the entity properties with data from the DTO
            oldPrs.Name = fNewDTO.Name ?? oldPrs.Name ;
            oldPrs.Gender = fNewDTO.Gender ?? oldPrs.Gender;
            oldPrs.Address_City = fNewDTO.Address_City ?? oldPrs.Address_City ;
            oldPrs.Age = fNewDTO.Age ?? oldPrs.Age;
            oldPrs.Date = fNewDTO.Date ?? oldPrs.Date;
            oldPrs.Note = fNewDTO.Note ?? oldPrs.Note;
            oldPrs.FoundCity = fNewDTO.FoundCity ?? oldPrs.FoundCity;
            oldPrs.PersonWhoFoundhim = fNewDTO.PersonWhoFoundhim ?? oldPrs.PersonWhoFoundhim;
            oldPrs.PhonePersonWhoFoundhim = fNewDTO.PhonePersonWhoFoundhim ?? oldPrs.PhonePersonWhoFoundhim;
            // Handle image update
            if (fNewDTO.Image != null)
            {
                if (fNewDTO.Image.Length > MaxallwoedImageSize)
                    return BadRequest("Max allowed size for the image is 10 MB!");

                using var dataStream = new MemoryStream();
                await fNewDTO.Image.CopyToAsync(dataStream);
                oldPrs.Image = dataStream.ToArray();
            }

            // Save the changes to the database
            try
            {
                await _context.SaveChangesAsync();
                return Ok(fNewDTO);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                return StatusCode(500, "An error occurred while updating the entity.");
            }
        }


        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var foundPrs = await _context.foundPersons.FindAsync(id);
            if (foundPrs != null)
            {
                try
                {
                    _context.foundPersons.Remove(foundPrs);
                    _context.SaveChanges();
                    return Ok(foundPrs);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
            return BadRequest(ModelState);
        }
    }
}
