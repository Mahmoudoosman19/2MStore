using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Project.DAL.Entities.Order;

namespace Project.DAL.Data.Config
{
    internal class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.OwnsOne(orderItem => orderItem.ItemOrdered, product => product.WithOwner());

            builder.Property(orderItem => orderItem.Price)
                .HasColumnType("decimal(18,2)");
        }
    }
}
