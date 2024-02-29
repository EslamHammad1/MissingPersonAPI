namespace Missingpreson
{
    public class FoundPerson : BaseEntity
    {
        public string FoundCity { get; set; } = string.Empty;
        public string PersonWhoFoundhim { get; set; } = string.Empty;
        public string PhonePersonWhoFoundhim { get; set; } = string.Empty;
    }
}
