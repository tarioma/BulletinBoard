using BulletinBoard.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BulletinBoard.Infrastructure.Context.Configurations;

public class BulletinConfiguration : IEntityTypeConfiguration<Bulletin>
{
    public void Configure(EntityTypeBuilder<Bulletin> builder)
    {
        builder.ToTable("bulletins");

        builder.HasKey(b => b.Id);

        builder.Property(p => p.Id)
            .HasColumnName("id")
            .IsRequired();

        builder.Property(p => p.Number)
            .HasColumnName("number")
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder.Property(p => p.Text)
            .HasColumnName("text")
            .HasMaxLength(Bulletin.MaxTextLength)
            .IsRequired();

        builder.Property(p => p.Rating)
            .HasColumnName("rating")
            .IsRequired();

        builder.Property(p => p.ExpiryUtc)
            .HasColumnName("expiry_utc")
            .HasColumnType("timestamp without time zone")
            .IsRequired();

        builder.Property(p => p.UserId)
            .HasColumnName("user_id")
            .IsRequired();

        builder.Property(p => p.CreatedUtc)
            .HasColumnName("created_utc")
            .HasColumnType("timestamp without time zone")
            .IsRequired();

        builder.Property(p => p.Image)
            .HasColumnName("image");

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(b => b.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasIndex(b => b.Number);
        builder.HasIndex(b => b.Text);
        builder.HasIndex(b => b.UserId);
        builder.HasIndex(b => b.Rating);
        builder.HasIndex(b => b.ExpiryUtc);
        builder.HasIndex(b => b.CreatedUtc);
    }
}