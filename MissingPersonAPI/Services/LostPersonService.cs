namespace MissingPersonAPI.Services
{
    public class LostPersonService : ILostPersonService
    {
        private readonly MissingPersonEntity _context;
        private long _MaxImageSizeAllowed = 10485760;
        private new List<string> _ValidExtensions = new List<string> { ".jpg , .png" };

        public LostPersonService(MissingPersonEntity context)
        {
            _context = context;
        }
        public async Task<IActionResult> GetAllLostPersons()
        {
            var LostPerson = await _context.lostPersons.OrderBy(x=> x.Name).ToListAsync();
            return new OkObjectResult(LostPerson);

        }

        public async Task<IActionResult> GetById(int id)
        {
            var LostPerson = await _context.lostPersons.FindAsync(id);
            if (LostPerson == null)
                return new NotFoundObjectResult(LostPerson);
            else
                return new OkObjectResult(LostPerson);
        }

        public async Task<IActionResult> PostLostPerson(LostPersonWithUserDTO lDTO)
        {
            var LostPerson = new LostPerson();

            if (lDTO.Image == null)
                return new BadRequestObjectResult("Image is Required !");
            if (lDTO.Image.Length > _MaxImageSizeAllowed)
                return new BadRequestObjectResult("Max allowed size for image is 10 MB! ");
            using var dataStream = new MemoryStream();
            await lDTO.Image.CopyToAsync(dataStream);
            LostPerson.Image = dataStream.ToArray();

            if (lDTO.Name == null)
                return new BadRequestObjectResult("Image is Required !");
            LostPerson.Name = lDTO.Name;

            if (lDTO.Age == null)
                return new BadRequestObjectResult("Age is Required !");

            LostPerson.Age = (int)lDTO.Age;

            if (lDTO.Gender == null)
                return new BadRequestObjectResult("Gender is Required !");

            LostPerson.Gender = lDTO.Gender;

            if (lDTO.Note == null)
                return new BadRequestObjectResult("Note is Required !");

            LostPerson.Note = lDTO.Note;

            if (lDTO.Date == null)
                return new BadRequestObjectResult("Date is Required !");

            LostPerson.Date = (DateTime)lDTO.Date;

            if (lDTO.Address_City == null)
                return new BadRequestObjectResult("Address_City is Required !");

            LostPerson.Address_City = lDTO.Address_City;

            if (lDTO.LostCity == null)
                return new BadRequestObjectResult("FoundCity is Required !");

            LostPerson.LostCity = lDTO.LostCity;

            if (lDTO.PersonWhoLost == null)
                return new BadRequestObjectResult("Finder is Required !");

            LostPerson.PersonWhoLost = lDTO.PersonWhoLost;

            if (lDTO.PhonePersonWhoLost == null)
                return new BadRequestObjectResult("FinderContact is Required !");

            LostPerson.PhonePersonWhoLost = lDTO.PhonePersonWhoLost;
            await _context.AddAsync(LostPerson);
            _context.SaveChanges();
            return new OkObjectResult(LostPerson);
        }
        public  async Task<IActionResult> Update(int id, LostPersonWithUserDTO lNewDTO)
        {
            // Find the existing FoundPeson entity by its ID
            var oldPrs = await _context.lostPersons.FindAsync(id);

            // Check if the entity exists
            if (oldPrs == null)
                return new NotFoundObjectResult($"Not Found {id}");

            // Update the entity properties with data from the DTO
            oldPrs.Name = lNewDTO.Name ?? oldPrs.Name;
            oldPrs.Gender = lNewDTO.Gender ?? oldPrs.Gender;
            oldPrs.Address_City = lNewDTO.Address_City ?? oldPrs.Address_City;
            oldPrs.Age = lNewDTO.Age ?? oldPrs.Age;
            oldPrs.Date = lNewDTO.Date ?? oldPrs.Date;
            oldPrs.Note = lNewDTO.Note ?? oldPrs.Note;
            oldPrs.LostCity = lNewDTO.LostCity ?? oldPrs.LostCity;
            oldPrs.PersonWhoLost = lNewDTO.PersonWhoLost ?? oldPrs.PersonWhoLost;
            oldPrs.PhonePersonWhoLost = lNewDTO.PhonePersonWhoLost ?? oldPrs.PhonePersonWhoLost;
            // Handle image update
            if (lNewDTO.Image != null)
            {
                if (lNewDTO.Image.Length > _MaxImageSizeAllowed)
                    return new BadRequestObjectResult("Max allowed size for the image is 10 MB!");

                using var dataStream = new MemoryStream();
                await lNewDTO.Image.CopyToAsync(dataStream);
                oldPrs.Image = dataStream.ToArray();
            }

            // Save the changes to the database
            try
            {
                await _context.SaveChangesAsync();
                return new OkObjectResult(lNewDTO);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                return new StatusCodeResult(500);
            }
        }
        public async Task<IActionResult> Delete(int id)
        {
            var LsotPrs = await _context.foundPersons.FindAsync(id);
            if (LsotPrs != null)
            {
                try
                {
                    _context.foundPersons.Remove(LsotPrs);
                    _context.SaveChanges();
                    return new OkObjectResult(LsotPrs);
                }
                catch (Exception ex)
                {
                    return new BadRequestObjectResult(ex.Message);
                }
            }
            return new BadRequestObjectResult("Not Found");
        }
    }
}
