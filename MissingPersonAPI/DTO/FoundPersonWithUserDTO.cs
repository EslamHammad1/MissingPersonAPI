﻿namespace MissingPersonAPI
{
    public class FoundPersonWithUserDTO
    {
        public string? Name { get; set; } 
        public int? Age { get; set; }
        public string? Gender { get; set; } 
        public string? Note { get; set; } 
        public DateTime? Date { get; set; }
        public string? FoundCity { get; set; }  
        public string? Address_City { get; set; } 
        public IFormFile? Image { get; set; }
        public string? Finder { get; set; } 
        public string? FinderContact { get; set; }
    }
}
