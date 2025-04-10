using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Project.DAL.Entities.Order;

namespace Project.DAL.Data.Config
{
    internal class DeliveryMethodConfigurations : IEntityTypeConfiguration<DeliveryMethod>
    {
        public void Configure(EntityTypeBuilder<DeliveryMethod> builder)
        {
            builder.Property(deliverymethod => deliverymethod.Cost)
                .HasColumnType("decimal(18,2)");



        }
    }
}
