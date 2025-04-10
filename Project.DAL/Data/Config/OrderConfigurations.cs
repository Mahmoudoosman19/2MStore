using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Project.DAL.Entities.Order;

namespace Project.DAL.Data.Config
{
    public class OrderConfigurations : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.OwnsOne(o => o.ShippingAddress, x => x.WithOwner());  // one to one relation

            builder.Property(o => o.Status)
                .HasConversion(
                OStatus => OStatus.ToString(),
                OStatus => (OrderStatus)Enum.Parse(typeof(OrderStatus), OStatus)
                );
            builder.Property(o => o.OrderTrackingStatus)
                .HasConversion(
                OStatus => OStatus.ToString(),
                OStatus => (OrderTracking)Enum.Parse(typeof(OrderTracking), OStatus)
                );

            builder.Property(o => o.SubTotal)
                .HasColumnType("decimal(18,2)");

            builder.HasOne(o => o.DeliveryMethod)
                .WithMany()
                .OnDelete(DeleteBehavior.SetNull);
        }

    }
}
