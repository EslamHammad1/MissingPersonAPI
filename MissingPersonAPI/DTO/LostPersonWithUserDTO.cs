﻿namespace MissingPersonAPI
{
    public class LostPersonWithUserDTO
    {
        public string? Name { get; set; } 
        public int? Age { get; set; }
        public string? Note { get; set; }
        public string? Gender { get; set; }
        public DateTime? Date { get; set; }
        public string?  LostCity { get; set; }
        public string? Address_City { get; set; }
        public IFormFile? Image { get; set; }
        public string? PersonWhoLost { get; set; } 
        public string? PhonePersonWhoLost { get; set; }
    }
}
