using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Project.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.DAL.Data.Config
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.Property(P => P.Name).IsRequired().HasMaxLength(100);
            builder.Property(P => P.Description).IsRequired();
            builder.Property(P => P.PriceBeforeDiscount).HasColumnType("decimal(18,2)");
            builder.Property(P => P.Discount).HasColumnType("decimal(18,2)");
            builder.Property(P => P.PictureUrl).IsRequired();
        }
    }
}
