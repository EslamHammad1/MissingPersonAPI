using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Missingpreson;
using Missingpreson.Models;
namespace Missingpreson.DataEF
{

    public class MissingPersonEntity : IdentityDbContext<ApplicationUser>
    {
        public MissingPersonEntity()
        {
            
        }
        public MissingPersonEntity(DbContextOptions options) : base (options)
        {
            
        }
        public DbSet<FoundPerson> foundPersons { get; set; }
        public DbSet<LostPerson> lostPersons { get; set; }

    }
}
