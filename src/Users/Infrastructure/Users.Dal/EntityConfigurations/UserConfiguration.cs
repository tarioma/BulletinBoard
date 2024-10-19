using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Users.Domain.Entities;

namespace Users.Dal.EntityConfigurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable(nameof(User));

        builder.HasKey(b => b.Id);

        builder.Property(p => p.Id).IsRequired();
        builder.Property(p => p.Name).IsRequired();
        builder.Property(p => p.IsAdmin).IsRequired();
        builder.Property(p => p.CreatedUtc).IsRequired();

        builder.HasIndex(b => b.Name);
        builder.HasIndex(b => b.IsAdmin);
        builder.HasIndex(b => b.CreatedUtc);
    }
}