using BulletinBoard.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BulletinBoard.Dal.EntityConfigurations;

public class BulletinConfiguration : IEntityTypeConfiguration<Bulletin>
{
    public void Configure(EntityTypeBuilder<Bulletin> builder)
    {
        builder.ToTable(nameof(Bulletin));

        builder.HasKey(b => b.Id);

        builder.Property(p => p.Id).IsRequired();
        builder.Property(p => p.Number).ValueGeneratedOnAdd().IsRequired();
        builder.Property(p => p.Text).IsRequired();
        builder.Property(p => p.ExpiryUtc).IsRequired();
        builder.Property(p => p.Image);
        builder.Property(p => p.UserId).IsRequired();
        builder.Property(p => p.CreatedUtc).IsRequired();

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(b => b.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasIndex(b => b.Number);
        builder.HasIndex(b => b.Text);
        builder.HasIndex(b => b.UserId);
        builder.HasIndex(b => b.ExpiryUtc);
        builder.HasIndex(b => b.CreatedUtc);
    }
}