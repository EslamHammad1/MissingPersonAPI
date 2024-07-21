namespace MissingPersonAPI.Services
{
    public interface ISearchService
    {
        IActionResult SearchForLostByName(SearchNameDTO searchDTO); 
        IActionResult SearchForFoundByName(SearchNameDTO searchDTO);
        IActionResult SearchForLostByCity(SearchCityDTO searchDTO);
        IActionResult SearchForFoundByCity(SearchCityDTO searchDTO);

    }
}
