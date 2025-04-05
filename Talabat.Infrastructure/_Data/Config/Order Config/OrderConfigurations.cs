using Talabat.Core.Entities.Order_Aggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Infrastructure._Data.Config.Order_Config
{
    internal class OrderConfigurations : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.OwnsOne(o => o.ShippingAddress, SA => SA.WithOwner());
            builder.Property(o => o.Status).HasConversion(
                (oStatus) => oStatus.ToString(),
                (oStatus) => (OrderStatus)Enum.Parse(typeof(OrderStatus), oStatus)
                );
            builder.Property(o=>o.Subtotal).HasColumnType("decimal(12,2)");
            builder.HasOne(o => o.DeliveryMethod).WithMany().HasForeignKey(o => o.DeliveryMethodId).OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(o => o.Items).WithOne().OnDelete(DeleteBehavior.Cascade);

            //builder.HasOne(o => o.DeliveryMethod).WithOne();
            //builder.HasIndex("DeliveryMethodId").IsUnique(true);

        }
    }
}
