using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Users.Core.Entities;
using Users.Core.ValueObjects;

namespace Users.Persistence.Configurations;

internal sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(x => x.Gid);

        builder.HasIndex(x => x.Email);

        builder.HasIndex(x => x.UserName);

        builder.Property(x => x.UserName)
               .HasConversion(x => x!.Value, x => UserName.Init(x))
               .HasMaxLength(20)
               .IsRequired();

        builder.Property(x => x.Email)
               .HasConversion(x => x!.Value, x => Email.Init(x))
               .IsRequired();

        builder.Property(x => x.Password)
               .HasMaxLength(150)
               .IsRequired();

        builder.HasOne(x => x.Role)
               .WithMany(x => x.Users)
               .HasForeignKey(x => x.RoleGid);
    }
}