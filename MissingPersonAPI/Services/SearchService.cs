
using Microsoft.AspNetCore.Http.HttpResults;

namespace MissingPersonAPI.Services
{
    public class SearchService : ISearchService
    {
        private readonly MissingPersonEntity _context;
        public SearchService(MissingPersonEntity context)
        {
           _context = context;
        }
        public IActionResult SearchForLostByName(SearchNameDTO searchDTO)
        {
            IQueryable<LostPerson> query = _context.lostPersons.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchDTO.Name))
            {
                query = query.Where(p => p.Name.StartsWith(searchDTO.Name));
            }

            var results = query.ToList();
            return new OkObjectResult(results);
        }

        public IActionResult SearchForLostByCity(SearchCityDTO searchDTO)
        {
            IQueryable<LostPerson> query = _context.lostPersons.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchDTO.Address_City))
            {
                query = query.Where(p => p.Address_City.Contains(searchDTO.Address_City));
            }

            var results = query.ToList();
            return new OkObjectResult(results);
        }
        public IActionResult SearchForFoundByName(SearchNameDTO searchDTO)
        {

            IQueryable<FoundPerson> query = _context.foundPersons.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchDTO.Name))
            {
                query = query.Where(p => p.Name.StartsWith(searchDTO.Name));
            }

            var results = query.ToList();
            return new OkObjectResult(results);
        }
        public IActionResult SearchForFoundByCity(SearchCityDTO searchDTO)
        {
            IQueryable<FoundPerson> query = _context.foundPersons.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchDTO.Address_City))
            {
                query = query.Where(p => p.Address_City.Contains(searchDTO.Address_City));
            }

            var results = query.ToList();
            return new OkObjectResult(results);
        }

      

      

       
    }
}
