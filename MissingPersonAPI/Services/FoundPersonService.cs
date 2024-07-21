namespace MissingPersonAPI.Services
{
    public class FoundPersonService : IFoundPersonService
    {
        private readonly MissingPersonEntity _context;
        private long _MaxImageSizeAllowed = 10485760; 
        private new List<string> _ValidExtensions = new List<string> { ".jpg , .png" };

        public FoundPersonService(MissingPersonEntity context  )
        {
            _context = context;
        }
        public async Task<IActionResult> GetAllFoundPerson()
        {
            var foundPersons = await _context.foundPersons.OrderBy(x => x.Name).ToListAsync();
            return new OkObjectResult(foundPersons);

        }

        public async Task<IActionResult> GetById(int id)
        {
            var foundPrs = await _context.foundPersons.FindAsync(id);
            if(foundPrs == null)
                return new NotFoundObjectResult(foundPrs);
            else
                return new OkObjectResult(foundPrs);
        }
        public async Task<IActionResult> PostFoundPerson(FoundPersonWithUserDTO fDTO)
        {
            var FoundPerson = new FoundPerson();

            if (fDTO.Image == null)
                return new BadRequestObjectResult("Image is Required !");
            if (fDTO.Image.Length > _MaxImageSizeAllowed)
                return new BadRequestObjectResult("Max allowed size for image is 10 MB! ");
            using var dataStream = new MemoryStream();
            await fDTO.Image.CopyToAsync(dataStream);
            FoundPerson.Image = dataStream.ToArray();

                if (fDTO.Name == null)
                return new BadRequestObjectResult("Name is Required !");
              FoundPerson.Name = fDTO.Name;

            if (fDTO.Age == null)
                return new BadRequestObjectResult("Age is Required !");
            
                FoundPerson.Age = (int)fDTO.Age; 

            if (fDTO.Gender == null)
                return new BadRequestObjectResult("Gender is Required !");
            
                FoundPerson.Gender = fDTO.Gender;

            if (fDTO.Note == null)
                return new BadRequestObjectResult("Note is Required !");
            
                FoundPerson.Note = fDTO.Note; 

            if (fDTO.Date == null)
                return new BadRequestObjectResult("Date is Required !");
            
                FoundPerson.Date = (DateTime)fDTO.Date;

            if (fDTO.Address_City == null)
                return new BadRequestObjectResult("Address_City is Required !");

            FoundPerson.Address_City = fDTO.Address_City;

            if (fDTO.FoundCity == null)
                return new BadRequestObjectResult("FoundCity is Required !");

            FoundPerson.FoundCity = fDTO.FoundCity;

            if (fDTO.Finder== null)
                return new BadRequestObjectResult("Finder is Required !");

            FoundPerson.PersonWhoFoundhim = fDTO.Finder;

            if (fDTO.FinderContact== null)
                return new BadRequestObjectResult("FinderContact is Required !");

            FoundPerson.PhonePersonWhoFoundhim = fDTO.FinderContact;
            await _context.AddAsync(FoundPerson);
            _context.SaveChanges();
            return new OkObjectResult(FoundPerson);

        }

        public async Task<IActionResult> Update(int id, FoundPersonWithUserDTO fNewDTO)
        {
            // Find the existing FoundPeson entity by its ID
            var oldPrs = await _context.foundPersons.FindAsync(id);

            // Check if the entity exists
            if (oldPrs == null)
                return new NotFoundObjectResult($"Not Found {id}");

            // Update the entity properties with data from the DTO
            oldPrs.Name = fNewDTO.Name ?? oldPrs.Name;
            oldPrs.Gender = fNewDTO.Gender ?? oldPrs.Gender;
            oldPrs.Address_City = fNewDTO.Address_City ?? oldPrs.Address_City;
            oldPrs.Age = fNewDTO.Age ?? oldPrs.Age;
            oldPrs.Date = fNewDTO.Date ?? oldPrs.Date;
            oldPrs.Note = fNewDTO.Note ?? oldPrs.Note;
            oldPrs.FoundCity= fNewDTO.FoundCity ?? oldPrs.FoundCity;
            oldPrs.PersonWhoFoundhim = fNewDTO.Finder ?? oldPrs.PersonWhoFoundhim;
            oldPrs.PhonePersonWhoFoundhim = fNewDTO.FinderContact ?? oldPrs.PhonePersonWhoFoundhim;
            // Handle image update
            if (fNewDTO.Image != null)
            {
                if (fNewDTO.Image.Length > _MaxImageSizeAllowed)
                    return new BadRequestObjectResult("Max allowed size for the image is 10 MB!");

                using var dataStream = new MemoryStream();
                await fNewDTO.Image.CopyToAsync(dataStream);
                oldPrs.Image = dataStream.ToArray();
            }

            // Save the changes to the database
            try
            {
                await _context.SaveChangesAsync();
                return new OkObjectResult(fNewDTO);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                return new StatusCodeResult(500);
            }
        }
        public async  Task<IActionResult> Delete(int id)
        {
            var foundPrs = await _context.foundPersons.FindAsync(id);
            if (foundPrs != null)
            {
                try
                {
                    _context.foundPersons.Remove(foundPrs);
                    _context.SaveChanges();
                    return new OkObjectResult(foundPrs);
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
