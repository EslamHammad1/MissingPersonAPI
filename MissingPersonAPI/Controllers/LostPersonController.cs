﻿using Microsoft.AspNetCore.JsonPatch;

namespace Test_1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class LostPersonController : ControllerBase
    {
        private new List<string> allowedExtensions = new List<string> { ".jpg , .png" }; // new
        private long MaxallwoedImageSize = 10485760; // new
        private readonly MissingPersonEntity _context;
        public LostPersonController(MissingPersonEntity context)
        {
            _context = context; 
        }
        [HttpGet]
        public IActionResult GetAllMissingperson()
        {
            List<LostPerson> missList = _context.lostPersons.ToList();
            return Ok(missList);
        }
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetbyID(int id)
        {


            var missPrs = await _context.lostPersons.FindAsync(id);
            FoundPersonWithUserDTO fDTO = new FoundPersonWithUserDTO();
            if (missPrs == null)
                return NotFound();
            else
                return Ok(missPrs);
        }

        [HttpPost]
        public async Task<IActionResult> PostLostperson([FromForm] LostPersonWithUserDTO lDTO)

        {
                if (lDTO.Image == null)
                    return BadRequest("Image is Required !");
                if (lDTO.Image.Length > MaxallwoedImageSize)
                    return BadRequest("Max allowed size for image is 10 MB! ");
                using var dataStreem = new MemoryStream();
                await lDTO.Image.CopyToAsync(dataStreem);
                var LostPerson = new LostPerson
                {
                    Name = lDTO.Name,
                    Age = (int)lDTO.Age,
                    Gender = lDTO.Gender,
                    Image = dataStreem.ToArray(), // new
                    Note = lDTO.Note,
                    LostCity = lDTO.LostCity,
                    Address_City = lDTO.Address_City,
                    Date = (DateTime)lDTO.Date,
                    PersonWhoLost = lDTO.PersonWhoLost,
                    PhonePersonWhoLost = lDTO.PhonePersonWhoLost,
                };
                await _context.AddAsync(LostPerson);
                _context.SaveChanges();
                return Ok(LostPerson);
    
        }


        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdatePerson(int id, [FromForm] LostPersonWithUserDTO lNewDTO)
        {
            var oldPrs = await _context.lostPersons.FindAsync(id);
            if (oldPrs == null)
                return NotFound($"Not Found{id}");
            
            // Store a copy of the old data
            var oldData = new LostPersonWithUserDTO
            {
                Name = oldPrs.Name,
                Gender = oldPrs.Gender,
                Address_City = oldPrs.Address_City,
                Age = oldPrs.Age,
                Date = oldPrs.Date,
                LostCity = oldPrs.LostCity,
                Note = oldPrs.Note,
                PersonWhoLost = oldPrs.PersonWhoLost,
                PhonePersonWhoLost = oldPrs.PhonePersonWhoLost,
            };

            if (lNewDTO.Image != null)
            {
                if (lNewDTO.Image.Length > MaxallwoedImageSize)
                    return BadRequest("Max allowed size for image is 10 MB!");

                using (var dataStreem = new MemoryStream())
                {
                    await lNewDTO.Image.CopyToAsync(dataStreem);
                    oldPrs.Image = dataStreem.ToArray();
                }
            }
            oldPrs.Name = lNewDTO.Name ?? oldPrs.Name;
            oldPrs.Gender = lNewDTO.Gender ?? oldPrs.Gender;
            oldPrs.Address_City = lNewDTO.Address_City ?? oldPrs.Address_City;
            oldPrs.Age = lNewDTO.Age ?? oldPrs.Age;
            oldPrs.Date = lNewDTO.Date ?? oldPrs.Date;
            oldPrs.Note = lNewDTO.Note ?? oldPrs.Note;
            oldPrs.LostCity = lNewDTO.LostCity ?? oldPrs.LostCity;
            oldPrs.PersonWhoLost = lNewDTO.PersonWhoLost ?? oldPrs.PersonWhoLost;
            oldPrs.PhonePersonWhoLost = lNewDTO.PhonePersonWhoLost ?? oldPrs.PhonePersonWhoLost;

            _context.Entry(oldPrs).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            // Return a custom response object containing both the old and updated data
            var responseData = new
            {
                OldData = oldData,
                NewData = lNewDTO
            };

            return Ok(responseData);
        }

        [HttpDelete("{id:int}")]
        //    [Authorize]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var lostPrs = await _context.lostPersons.FindAsync(id);
            if (lostPrs != null)
            {
                try
                {
                    _context.lostPersons.Remove(lostPrs);
                    _context.SaveChanges();
                    return Ok(lostPrs);
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
