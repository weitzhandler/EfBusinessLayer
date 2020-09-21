using Microsoft.EntityFrameworkCore;

namespace EfBusinessLayer
{
    public class AppDbContext : DbContext, IRepository<Contact>
    {
        public DbSet<Contact> Contacts { get; set; }

        DbSet<Contact> IRepository<Contact>.Set() => Contacts;
    }
}
