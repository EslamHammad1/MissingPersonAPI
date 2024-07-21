namespace MissingPersonAPI.Services
{
    public interface IFoundPersonService
    {
        Task<IActionResult> GetAllFoundPerson();
        Task<IActionResult> GetById(int id);
        Task<IActionResult> PostFoundPerson(FoundPersonWithUserDTO fDTO);
        Task<IActionResult> Update(int id , FoundPersonWithUserDTO fNewDTO);
        Task<IActionResult> Delete(int id);
    }

}
