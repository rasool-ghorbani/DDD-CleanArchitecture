using Domain.Aggregates.Customer;
using Domain.Aggregates.Customer.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configuration
{
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.HasKey(c => c.Id);

            // ValueObjectها
            builder.OwnsOne(c => c.FirstName, b =>
            {
                b.Property(v => v.Value)
                 .HasColumnName("FirstName")
                 .HasMaxLength(100)
                 .IsRequired();
            });

            builder.OwnsOne(c => c.LastName, b =>
            {
                b.Property(v => v.Value)
                 .HasColumnName("LastName")
                 .HasMaxLength(100)
                 .IsRequired();
            });

            builder.OwnsOne(c => c.Email, b =>
            {
                b.Property(v => v.Value)
                 .HasColumnName("Email")
                 .HasMaxLength(200)
                 .IsRequired();
            });

            builder.OwnsOne(c => c.DateOfBirth, b =>
            {
                b.Property(v => v.Value)
                 .HasColumnName("DateOfBirth")
                 .IsRequired();
            });

            builder.OwnsOne(c => c.BankAccountNumber, b =>
            {
                b.Property(v => v.Value)
                 .HasColumnName("BankAccountNumber")
                 .IsRequired();
            });

            builder.OwnsOne(u => u.PhoneNumber, pn =>
            {
                pn.Property(p => p.CountryCode)
                  .HasColumnName("PhoneCountryCode")
                  .HasMaxLength(5)
                  .IsRequired();

                pn.Property(p => p.Number)
                  .HasColumnName("PhoneNumber")
                  .HasMaxLength(15)
                  .IsRequired();
            });

            builder.Property<bool>(a=>a.IsDeleted)
                   .IsRequired();
        }
    }
}
