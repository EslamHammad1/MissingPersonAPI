namespace MissingPersonAPI.Services
{
    public interface ILostPersonService
    {
        Task<IActionResult> GetAllLostPersons();
        Task<IActionResult> GetById(int id);
        Task<IActionResult> PostLostPerson(LostPersonWithUserDTO lDTO);
        Task<IActionResult> Update(int id, LostPersonWithUserDTO lDTO);
        Task<IActionResult> Delete(int id);
    }
}
