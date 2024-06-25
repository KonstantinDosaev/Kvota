using Kvota.Models;
using Kvota.Models.UserAuth;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Kvota.Migrations;

public class KvotaContext : IdentityDbContext<KvotaUser>
{
    public KvotaContext(DbContextOptions<KvotaContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<KvotaUser>()
            .HasQueryFilter(x => x.IsDeleted == false);

        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
    }

    //public virtual DbSet<Home> Homes { get; set; } = null!;
    //public virtual DbSet<ContactsModel> Contacts { get; set; } = null!;
    public virtual DbSet<Address> Address { get; set; } = null!;

}
