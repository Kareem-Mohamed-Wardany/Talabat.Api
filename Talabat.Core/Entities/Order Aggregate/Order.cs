﻿namespace Talabat.Core.Entities.Order_Aggregate
{
    public class Order : BaseEntity
    {
        public string BuyerEmail { get; set; } = null!;
        public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.UtcNow;
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        public OrderAddress ShippingAddress { get; set; } = null!;

        public int? DeliveryMethodId { get; set; }
        public DeliveryMethod DeliveryMethod { get; set; } = null!;

        public ICollection<OrderItem> Items { get; set; } = new HashSet<OrderItem>();
        public decimal Subtotal { get; set; }

        //[NotMapped]
        //public decimal Total => Subtotal + DeliveryMethod.Cost; 

        public decimal GetTotal() => Subtotal + DeliveryMethod.Cost;

        public string PaymentIntentId { get; set; }

    }
}
