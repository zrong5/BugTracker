using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace TrackerData
{
    public class IdentityContext: IdentityDbContext
    {
        public IdentityContext(DbContextOptions options) : base(options) 
        {

        }
    }
}
