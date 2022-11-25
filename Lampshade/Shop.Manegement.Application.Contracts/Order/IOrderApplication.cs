using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopManagement.Application.Contracts.Order
{
    public interface IOrderApplication
    {
        long PlaceOrder(Cart cart);
        string PaymentSucceeded(long id, long refId);
        void Cancel(long id);
        List<OrderViewModel> Search(OrderSearchModel searchModel);
        List<OrderItemViewModel> GetItems(long orderId);
        double GetAmountBy(long id);

    }
}
