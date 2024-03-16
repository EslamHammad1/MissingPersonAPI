namespace Missingpreson
{
    public class UserRegisterDTO
    {
        public string? UserName { get; set; } 
        public string? Name { get; set; } 
        [DataType(DataType.Password)]
        public string? Password { get; set; } 
        [Compare("Password")]
        [DataType(DataType.Password)]
        public string? PasswordConfirmed { get; set; } 
    //    [EmailAddress]
        [AllowNull]
        public string? Email { get; set; } 

    }
}
