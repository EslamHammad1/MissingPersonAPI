namespace Missingpreson
{
    public class UserRegisterDTO
    {
        [Required]
        public string? UserName { get; set; } 
        public string? Name { get; set; } 
        [Required]
        [DataType(DataType.Password)]
        public string? Password { get; set; } 
        [Required]
        [Compare("Password")]
        [DataType(DataType.Password)]
        public string? PasswordConfirmed { get; set; } 
    //    [EmailAddress]
        [AllowNull]
        public string? Email { get; set; } 

    }
}
