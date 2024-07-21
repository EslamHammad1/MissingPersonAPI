namespace MissingPersonAPI
{
    public class UserLoginDTO
    {
        public string UserName { get; set; } 
        [DataType(DataType.Password)]
        public string Password { get; set; } 
   
    }
}
