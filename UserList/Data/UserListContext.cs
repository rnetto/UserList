using Microsoft.EntityFrameworkCore;
using UserList.Models;

namespace UserList.Data
{
    public class UserListContext : DbContext
    {
        public UserListContext (DbContextOptions<UserListContext> options)
            : base(options)
        {
        }

        public DbSet<Usuario> Usuario { get; set; }
    }
}
