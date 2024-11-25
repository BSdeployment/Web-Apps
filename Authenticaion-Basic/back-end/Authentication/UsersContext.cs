using Microsoft.EntityFrameworkCore;

namespace ApiWuthHush.Authentication
{
    public class UsersContext : DbContext
    {
        public UsersContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
    }
}
