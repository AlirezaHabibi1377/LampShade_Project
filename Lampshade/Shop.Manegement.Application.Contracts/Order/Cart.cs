using System.Collections.Generic;

namespace ShopManagement.Application.Contracts.Order
{
    public class Cart
    {
        public List<CartItem> Items { get; set; }
        public double TotalAmount { get; set; }
        public double DiscountAmount { get; set; }
        public int PaymentMethod { get; set; }

        public double PayAmount { get; set; }

        public Cart()
        {
            Items = new List<CartItem>();
        }

        public void Add(CartItem cartItem)
        {
            Items.Add(cartItem);
            TotalAmount = cartItem.TotalItemPrice + TotalAmount;
            DiscountAmount = cartItem.DiscountAmount + DiscountAmount;
            PayAmount = cartItem.ItemPayAmount + PayAmount;
        }


        public void SetPaymentMethod(int methodId)
        {
            PaymentMethod = methodId;
        }
    }
}